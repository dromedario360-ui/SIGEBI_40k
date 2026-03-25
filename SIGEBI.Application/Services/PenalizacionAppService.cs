using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.DomainServices;
using SIGEBI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Application.Services
{
    public class PenalizacionAppService : IPenalizacionAppService
    {
        private readonly IPenalizacionRepository _repo;
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly PenalizacionDomainService _domainSvc;

        public PenalizacionAppService(IPenalizacionRepository repo, IUsuarioRepository usuarioRepo,
                                       PenalizacionDomainService domainSvc)
        {
            _repo = repo;
            _usuarioRepo = usuarioRepo;
            _domainSvc = domainSvc;
        }

        public async Task<Result<PenalizacionResponse>> CrearAsync(CrearPenalizacionRequest req)
        {
            var result = _domainSvc.CrearPenalizacion(req.IdUsuario, req.Motivo, req.Monto);
            if (!result.IsSuccess)
                return Result<PenalizacionResponse>.Failure(result.Error!);

            await _repo.AgregarAsync(result.Value!);
            return Result<PenalizacionResponse>.Success(await MapearAsync(result.Value!));
        }

        public async Task<Result<IEnumerable<PenalizacionResponse>>> ObtenerTodasAsync()
        {
            var lista = await _repo.ObtenerTodosAsync();
            var mapped = new List<PenalizacionResponse>();
            foreach (var p in lista) mapped.Add(await MapearAsync(p));
            return Result<IEnumerable<PenalizacionResponse>>.Success(mapped);
        }

        public async Task<Result<IEnumerable<PenalizacionResponse>>> ObtenerPorUsuarioAsync(int idUsuario)
        {
            var lista = await _repo.ObtenerActivasPorUsuarioAsync(idUsuario);
            var mapped = new List<PenalizacionResponse>();
            foreach (var p in lista) mapped.Add(await MapearAsync(p));
            return Result<IEnumerable<PenalizacionResponse>>.Success(mapped);
        }

        public async Task<Result> FinalizarAsync(int id)
        {
            var pen = await _repo.ObtenerPorIdAsync(id);
            if (pen is null) return Result.Failure("Penalización no encontrada.");

            var result = _domainSvc.FinalizarPenalizacion(pen);
            if (!result.IsSuccess) return result;

            await _repo.ActualizarAsync(pen);
            return Result.Success();
        }

        private async Task<PenalizacionResponse> MapearAsync(Domain.Entities.Penalizacion p)
        {
            var usuario = await _usuarioRepo.ObtenerPorIdAsync(p.IdUsuario);
            return new PenalizacionResponse(p.Id, p.IdUsuario,
                usuario?.Nombre.NombreCompleto ?? "", p.Motivo,
                p.Monto.Monto, p.FechaInicio, p.FechaFin, p.Activa);
        }
    }
}
