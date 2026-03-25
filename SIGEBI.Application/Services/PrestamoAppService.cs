using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.DomainServices;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Application.Services
{
    public class PrestamoAppService : IPrestamoAppService
    {
        private readonly IPrestamoRepository _prestamoRepo;
        private readonly IRecursoRepository _recursoRepo;
        private readonly IPenalizacionRepository _penRepo;
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IAuditoriaRepository _auditoriaRepo;
        private readonly PrestamoDomainService _domainSvc;
        private readonly PenalizacionDomainService _penDomainSvc;

        public PrestamoAppService(IPrestamoRepository prestamoRepo, IRecursoRepository recursoRepo,
                                   IPenalizacionRepository penRepo, IUsuarioRepository usuarioRepo,
                                   IAuditoriaRepository auditoriaRepo, PrestamoDomainService domainSvc,
                                   PenalizacionDomainService penDomainSvc)
        {
            _prestamoRepo = prestamoRepo;
            _recursoRepo = recursoRepo;
            _penRepo = penRepo;
            _usuarioRepo = usuarioRepo;
            _auditoriaRepo = auditoriaRepo;
            _domainSvc = domainSvc;
            _penDomainSvc = penDomainSvc;
        }

        public async Task<Result<PrestamoResponse>> CrearAsync(CrearPrestamoRequest req)
        {
            var tienePen = await _penDomainSvc.UsuarioTienePenalizacionActivaAsync(req.IdUsuario);
            if (tienePen.Value)
                return Result<PrestamoResponse>.Failure("El usuario tiene una penalización activa.");

            var result = _domainSvc.CrearPrestamo(req.IdUsuario, req.FechaLimite);
            if (!result.IsSuccess)
                return Result<PrestamoResponse>.Failure(result.Error!);

            var prestamo = result.Value!;
            await _prestamoRepo.AgregarAsync(prestamo);

            foreach (var det in req.Detalles)
            {
                var recurso = await _recursoRepo.ObtenerPorIdAsync(det.IdRecurso);
                if (recurso is null)
                    return Result<PrestamoResponse>.Failure($"Recurso {det.IdRecurso} no encontrado.");

                var detResult = _domainSvc.AgregarDetalle(prestamo, recurso, det.Cantidad);
                if (!detResult.IsSuccess)
                    return Result<PrestamoResponse>.Failure(detResult.Error!);

                await _recursoRepo.ActualizarAsync(recurso);
            }

            await _auditoriaRepo.AgregarAsync(
                Auditoria.Crear(req.IdUsuario, "CREAR_PRESTAMO", $"Préstamo creado para usuario {req.IdUsuario}"));

            return Result<PrestamoResponse>.Success(await MapearAsync(prestamo));
        }

        public async Task<Result<PrestamoResponse>> ObtenerPorIdAsync(int id)
        {
            var p = await _prestamoRepo.ObtenerConDetallesAsync(id);
            if (p is null) return Result<PrestamoResponse>.Failure("Préstamo no encontrado.");
            return Result<PrestamoResponse>.Success(await MapearAsync(p));
        }

        public async Task<Result<IEnumerable<PrestamoResponse>>> ObtenerTodosAsync()
        {
            var lista = await _prestamoRepo.ObtenerTodosAsync();
            var mapped = new List<PrestamoResponse>();
            foreach (var p in lista) mapped.Add(await MapearAsync(p));
            return Result<IEnumerable<PrestamoResponse>>.Success(mapped);
        }

        public async Task<Result> ProcesarDevolucionAsync(int id)
        {
            var prestamo = await _prestamoRepo.ObtenerConDetallesAsync(id);
            if (prestamo is null) return Result.Failure("Préstamo no encontrado.");

            var recursos = new List<Domain.Entities.Recursos.RecursoBase>();
            foreach (var det in prestamo.Detalles)
            {
                var r = await _recursoRepo.ObtenerPorIdAsync(det.IdRecurso);
                if (r != null) recursos.Add(r);
            }

            var result = _domainSvc.ProcesarDevolucion(prestamo, recursos);
            if (!result.IsSuccess) return result;

            if (prestamo.TotalMulta.Monto > 0)
            {
                var pen = _penDomainSvc.CrearPenalizacion(prestamo.IdUsuario,
                    "Devolución tardía", prestamo.TotalMulta.Monto);
                if (pen.IsSuccess)
                    await _penRepo.AgregarAsync(pen.Value!);
            }

            foreach (var r in recursos) await _recursoRepo.ActualizarAsync(r);
            await _prestamoRepo.ActualizarAsync(prestamo);
            await _auditoriaRepo.AgregarAsync(
                Auditoria.Crear(prestamo.IdUsuario, "DEVOLUCION",
                    $"Préstamo {id} devuelto. Multa: {prestamo.TotalMulta}"));

            return Result.Success();
        }

        public async Task<Result> MarcarVencidosAsync()
        {
            var vencidos = await _prestamoRepo.ObtenerVencidosAsync();
            foreach (var p in vencidos)
            {
                var result = _domainSvc.MarcarVencido(p);
                if (result.IsSuccess)
                {
                    await _prestamoRepo.ActualizarAsync(p);
                    await _auditoriaRepo.AgregarAsync(
                        Auditoria.Crear(null, "PRESTAMO_VENCIDO", $"Préstamo {p.Id} marcado como vencido."));
                }
            }
            return Result.Success();
        }

        private async Task<PrestamoResponse> MapearAsync(Prestamo p)
        {
            var usuario = await _usuarioRepo.ObtenerPorIdAsync(p.IdUsuario);
            var detalles = new List<DetallePrestamoResponse>();
            foreach (var d in p.Detalles)
            {
                var recurso = await _recursoRepo.ObtenerPorIdAsync(d.IdRecurso);
                detalles.Add(new DetallePrestamoResponse(d.IdRecurso, recurso?.Titulo ?? "", d.Cantidad));
            }
            return new PrestamoResponse(p.Id, p.IdUsuario, usuario?.Nombre.NombreCompleto ?? "",
                                         p.FechaPrestamo, p.FechaLimite, p.Estado.ToString(),
                                         p.TotalMulta.Monto, detalles);
        }
    }
}
