using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Interfaces;
using SIGEBI.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Domain.DomainServices
{
    public class PenalizacionDomainService
    {
        private readonly IPenalizacionRepository _repo;

        public PenalizacionDomainService(IPenalizacionRepository repo) => _repo = repo;

        public Result<Penalizacion> CrearPenalizacion(int idUsuario, string motivo, decimal monto)
        {
            if (string.IsNullOrWhiteSpace(motivo))
                return Result<Penalizacion>.Failure("El motivo es obligatorio.");
            if (monto < 0)
                return Result<Penalizacion>.Failure("El monto no puede ser negativo.");

            var penalizacion = Penalizacion.Crear(idUsuario, motivo, new Dinero(monto));
            return Result<Penalizacion>.Success(penalizacion);
        }

        public Result FinalizarPenalizacion(Penalizacion penalizacion)
        {
            if (!penalizacion.Activa)
                return Result.Failure("La penalización ya está finalizada.");

            penalizacion.Finalizar();
            return Result.Success();
        }

        public async Task<Result<bool>> UsuarioTienePenalizacionActivaAsync(int idUsuario)
        {
            var tiene = await _repo.UsuarioTienePenalizacionActivaAsync(idUsuario);
            return Result<bool>.Success(tiene);
        }
    }
}
