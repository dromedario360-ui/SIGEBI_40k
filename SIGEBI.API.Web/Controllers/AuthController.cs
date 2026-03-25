using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.DTOs.Request;

namespace SIGEBI.API.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioAppService _svc;
        private readonly IPasswordHasher _hasher;

        public AuthController(IUsuarioAppService svc, IPasswordHasher hasher)
        {
            _svc = svc;
            _hasher = hasher;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var usuario = await _svc.ObtenerTodosAsync();
            if (!usuario.IsSuccess)
                return BadRequest("Error al obtener usuarios.");

            var user = usuario.Value!
                .FirstOrDefault(u => u.Email.ToLower() == req.Email.ToLower());

            if (user is null)
                return Unauthorized("Credenciales inválidas.");

            var userCompleto = await _svc.ObtenerPorIdAsync(user.Id);
            if (!userCompleto.IsSuccess)
                return Unauthorized("Credenciales inválidas.");

            return Ok(new
            {
                mensaje = "Login exitoso",
                nombre = user.NombreCompleto,
                email = user.Email,
                rol = user.Rol
            });
        }
    }
}
