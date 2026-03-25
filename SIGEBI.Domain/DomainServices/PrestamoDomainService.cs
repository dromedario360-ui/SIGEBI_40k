using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Entities.Recursos;
using SIGEBI.Domain.Enums;
using SIGEBI.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.DomainServices
{
    public class PrestamoDomainService
    {
        private const decimal MultaPorDia = 50.00m;

        public Result<Prestamo> CrearPrestamo(int idUsuario, DateTime fechaLimite)
        {
            if (idUsuario <= 0)
                return Result<Prestamo>.Failure("El usuario es inválido.");
            if (fechaLimite.Date <= DateTime.Now.Date)
                return Result<Prestamo>.Failure("La fecha límite debe ser una fecha futura.");

            var prestamo = Prestamo.Crear(idUsuario, fechaLimite);
            return Result<Prestamo>.Success(prestamo);
        }

        public Result AgregarDetalle(Prestamo prestamo, RecursoBase recurso, int cantidad)
        {
            if (prestamo.Estado != EstadoPrestamo.ACTIVO)
                return Result.Failure("El préstamo no está activo.");

            if (!recurso.Disponible || recurso.Stock < cantidad)
                return Result.Failure($"Stock insuficiente para '{recurso.Titulo}'. Disponible: {recurso.Stock}.");

            recurso.ReducirStock(cantidad);
            prestamo.AgregarDetalle(recurso.Id, cantidad);

            return Result.Success();
        }

        public Result<Dinero> CalcularMulta(Prestamo prestamo, DateTime fechaDevolucion)
        {
            if (prestamo.Estado == EstadoPrestamo.DEVUELTO)
                return Result<Dinero>.Failure("El préstamo ya fue devuelto.");

            if (fechaDevolucion.Date <= prestamo.FechaLimite.Date)
                return Result<Dinero>.Success(new Dinero(0));

            var diasRetraso = (fechaDevolucion.Date - prestamo.FechaLimite.Date).Days;
            var multa = new Dinero(diasRetraso * MultaPorDia);
            return Result<Dinero>.Success(multa);
        }

        public Result ProcesarDevolucion(Prestamo prestamo, IEnumerable<RecursoBase> recursos)
        {
            if (prestamo.Estado == EstadoPrestamo.DEVUELTO)
                return Result.Failure("El préstamo ya fue devuelto.");

            var multaResult = CalcularMulta(prestamo, DateTime.Now);
            if (!multaResult.IsSuccess)
                return Result.Failure(multaResult.Error!);

            if (multaResult.Value!.Monto > 0)
                prestamo.AplicarMulta(multaResult.Value);

            foreach (var detalle in prestamo.Detalles)
            {
                var recurso = recursos.FirstOrDefault(r => r.Id == detalle.IdRecurso);
                recurso?.AumentarStock(detalle.Cantidad);
            }

            prestamo.MarcarComoDevuelto();
            return Result.Success();
        }

        public Result MarcarVencido(Prestamo prestamo)
        {
            if (prestamo.Estado != EstadoPrestamo.ACTIVO)
                return Result.Failure("Solo se pueden vencer préstamos activos.");
            if (DateTime.Now.Date <= prestamo.FechaLimite.Date)
                return Result.Failure("El préstamo aún no ha vencido.");

            prestamo.MarcarComoVencido();
            return Result.Success();
        }
    }
}
