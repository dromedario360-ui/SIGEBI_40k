using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Application.Services
{
    public class AuditoriaAppService : IAuditoriaAppService
    {
        private readonly IAuditoriaRepository _repo;
        private readonly IUsuarioRepository _usuarioRepo;

        public AuditoriaAppService(IAuditoriaRepository repo, IUsuarioRepository usuarioRepo)
        {
            _repo = repo;
            _usuarioRepo = usuarioRepo;
        }

        public async Task<Result<IEnumerable<AuditoriaResponse>>> ObtenerTodasAsync()
        {
            var lista = await _repo.ObtenerTodosAsync();
            return Result<IEnumerable<AuditoriaResponse>>.Success(await MapearListaAsync(lista));
        }

        public async Task<Result<IEnumerable<AuditoriaResponse>>> ObtenerPorUsuarioAsync(int idUsuario)
        {
            var lista = await _repo.ObtenerPorUsuarioAsync(idUsuario);
            return Result<IEnumerable<AuditoriaResponse>>.Success(await MapearListaAsync(lista));
        }

        public async Task<Result<IEnumerable<AuditoriaResponse>>> ObtenerPorFechaAsync(DateTime desde, DateTime hasta)
        {
            var lista = await _repo.ObtenerPorFechaAsync(desde, hasta);
            return Result<IEnumerable<AuditoriaResponse>>.Success(await MapearListaAsync(lista));
        }

        private async Task<IEnumerable<AuditoriaResponse>> MapearListaAsync(
            IEnumerable<Domain.Entities.Auditoria> lista)
        {
            var mapped = new List<AuditoriaResponse>();
            foreach (var a in lista)
            {
                string? nombre = null;
                if (a.IdUsuario.HasValue)
                {
                    var u = await _usuarioRepo.ObtenerPorIdAsync(a.IdUsuario.Value);
                    nombre = u?.Nombre.NombreCompleto;
                }
                mapped.Add(new AuditoriaResponse(a.Id, a.IdUsuario, nombre,
                    a.Accion, a.Descripcion, a.Fecha));
            }
            return mapped;
        }
    }
}
