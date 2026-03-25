using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response;
using SIGEBI.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Result<LoginResponse>> LoginAsync(LoginRequest request);
    }

    public interface IUsuarioAppService
    {
        Task<Result<UsuarioResponse>> CrearAsync(CrearUsuarioRequest request);
        Task<Result<UsuarioResponse>> ObtenerPorIdAsync(int id);
        Task<Result<IEnumerable<UsuarioResponse>>> ObtenerTodosAsync();
        Task<Result> DesactivarAsync(int id);
        Task<Result> ActualizarPerfilAsync(int id, ActualizarUsuarioRequest request);
    }

    public interface ICategoriaAppService
    {
        Task<Result<CategoriaResponse>> CrearAsync(CrearCategoriaRequest request);
        Task<Result<IEnumerable<CategoriaResponse>>> ObtenerTodasAsync();
        Task<Result<CategoriaResponse>> ActualizarAsync(int id, ActualizarCategoriaRequest request);
        Task<Result> DesactivarAsync(int id);
    }

    public interface IRecursoAppService
    {
        Task<Result<RecursoResponse>> CrearLibroAsync(CrearLibroRequest request);
        Task<Result<RecursoResponse>> CrearRevistaAsync(CrearRevistaRequest request);
        Task<Result<RecursoResponse>> ObtenerPorIdAsync(int id);
        Task<Result<IEnumerable<RecursoResponse>>> ObtenerTodosAsync();
        Task<Result<RecursoResponse>> ActualizarAsync(int id, ActualizarRecursoRequest request);
        Task<Result> AjustarStockAsync(int id, AjustarStockRequest request, bool esIncremento);
    }

    public interface IPrestamoAppService
    {
        Task<Result<PrestamoResponse>> CrearAsync(CrearPrestamoRequest request);
        Task<Result<PrestamoResponse>> ObtenerPorIdAsync(int id);
        Task<Result<IEnumerable<PrestamoResponse>>> ObtenerTodosAsync();
        Task<Result> ProcesarDevolucionAsync(int id);
        Task<Result> MarcarVencidosAsync();
    }

    public interface IPenalizacionAppService
    {
        Task<Result<PenalizacionResponse>> CrearAsync(CrearPenalizacionRequest request);
        Task<Result<IEnumerable<PenalizacionResponse>>> ObtenerTodasAsync();
        Task<Result<IEnumerable<PenalizacionResponse>>> ObtenerPorUsuarioAsync(int idUsuario);
        Task<Result> FinalizarAsync(int id);
    }

    public interface IAuditoriaAppService
    {
        Task<Result<IEnumerable<AuditoriaResponse>>> ObtenerTodasAsync();
        Task<Result<IEnumerable<AuditoriaResponse>>> ObtenerPorUsuarioAsync(int idUsuario);
        Task<Result<IEnumerable<AuditoriaResponse>>> ObtenerPorFechaAsync(DateTime desde, DateTime hasta);
    }

    public interface IPasswordHasher
    {
        string Hash(string password);
        bool Verify(string password, string hash);
    }

    public interface IJwtService
    {
        string GenerarToken(int idUsuario, string nombreCompleto, string email, string rol);
    }
}
