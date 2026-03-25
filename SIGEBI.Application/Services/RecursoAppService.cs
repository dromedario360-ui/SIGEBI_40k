using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Entities.Recursos;
using SIGEBI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Application.Services
{
    public class RecursoAppService : IRecursoAppService
    {
        private readonly IRecursoRepository _repo;
        private readonly IRepository<Categoria> _catRepo;

        public RecursoAppService(IRecursoRepository repo, IRepository<Categoria> catRepo)
        {
            _repo = repo;
            _catRepo = catRepo;
        }

        public async Task<Result<RecursoResponse>> CrearLibroAsync(CrearLibroRequest req)
        {
            var libro = Libro.Crear(req.IdCategoria, req.Codigo, req.Titulo,
                                    req.Autor, req.Stock, req.ISBN,
                                    req.Editorial, req.NumeroPaginas);
            await _repo.AgregarAsync(libro);
            return Result<RecursoResponse>.Success(await MapearAsync(libro));
        }

        public async Task<Result<RecursoResponse>> CrearRevistaAsync(CrearRevistaRequest req)
        {
            var revista = Revista.Crear(req.IdCategoria, req.Codigo, req.Titulo,
                                         req.Autor, req.Stock, req.ISBN,
                                         req.NumeroEdicion, req.Periodicidad);
            await _repo.AgregarAsync(revista);
            return Result<RecursoResponse>.Success(await MapearAsync(revista));
        }

        public async Task<Result<RecursoResponse>> ObtenerPorIdAsync(int id)
        {
            var r = await _repo.ObtenerPorIdAsync(id);
            if (r is null) return Result<RecursoResponse>.Failure("Recurso no encontrado.");
            return Result<RecursoResponse>.Success(await MapearAsync(r));
        }

        public async Task<Result<IEnumerable<RecursoResponse>>> ObtenerTodosAsync()
        {
            var lista = await _repo.ObtenerTodosAsync();
            var mapped = new List<RecursoResponse>();
            foreach (var r in lista) mapped.Add(await MapearAsync(r));
            return Result<IEnumerable<RecursoResponse>>.Success(mapped);
        }

        public async Task<Result<RecursoResponse>> ActualizarAsync(int id, ActualizarRecursoRequest req)
        {
            var recurso = await _repo.ObtenerPorIdAsync(id);
            if (recurso is null) return Result<RecursoResponse>.Failure("Recurso no encontrado.");

            recurso.Actualizar(req.Titulo, req.Autor, req.IdCategoria, req.ISBN);
            await _repo.ActualizarAsync(recurso);
            return Result<RecursoResponse>.Success(await MapearAsync(recurso));
        }

        public async Task<Result> AjustarStockAsync(int id, AjustarStockRequest req, bool esIncremento)
        {
            var recurso = await _repo.ObtenerPorIdAsync(id);
            if (recurso is null) return Result.Failure("Recurso no encontrado.");

            if (esIncremento)
                recurso.AumentarStock(req.Cantidad);
            else
                recurso.ReducirStock(req.Cantidad);

            await _repo.ActualizarAsync(recurso);
            return Result.Success();
        }

        private async Task<RecursoResponse> MapearAsync(RecursoBase r)
        {
            var cat = await _catRepo.ObtenerPorIdAsync(r.IdCategoria);
            return new RecursoResponse(r.Id, r.Codigo, r.Titulo, r.Autor, r.ISBN,
                                        r.Stock, r.Disponible, r.Tipo.ToString(),
                                        cat?.Nombre ?? "", r.FechaRegistro);
        }
    }
}
