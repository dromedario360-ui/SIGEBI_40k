using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Domain.Interfaces;

namespace SIGEBI.API.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioAppService _svc;
        private readonly IPasswordHasher _hasher;
        private readonly IUsuarioRepository _usuarioRepo;

        public AuthController(IUsuarioAppService svc, IPasswordHasher hasher, IUsuarioRepository usuarioRepo)
        {
            _svc = svc;
            _hasher = hasher;
            _usuarioRepo = usuarioRepo;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var usuario = await _usuarioRepo.ObtenerPorEmailAsync(req.Email);
            if (usuario is null)
                return Unauthorized("Credenciales inválidas.");

            if (!usuario.Activo)
                return Unauthorized("Usuario inactivo.");

            var passwordValido = _hasher.Verify(req.Password, usuario.PasswordHash);
            if (!passwordValido)
                return Unauthorized("Credenciales inválidas.");

            return Ok(new
            {
                mensaje = "Login exitoso",
                id = usuario.Id,
                nombre = usuario.Nombre.NombreCompleto,
                email = usuario.Email.ToString(),
                rol = usuario.IdRol
            });
        }
    }
}