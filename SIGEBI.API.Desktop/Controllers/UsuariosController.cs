using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.Interfaces;

namespace SIGEBI.API.Desktop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioAppService _svc;

        public UsuariosController(IUsuarioAppService svc) => _svc = svc;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var r = await _svc.ObtenerTodosAsync();
            return r.IsSuccess ? Ok(r.Value) : BadRequest(r.Error);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var r = await _svc.ObtenerPorIdAsync(id);
            return r.IsSuccess ? Ok(r.Value) : NotFound(r.Error);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearUsuarioRequest req)
        {
            var r = await _svc.CrearAsync(req);
            return r.IsSuccess ? Ok(r.Value) : BadRequest(r.Error);
        }

        [HttpPatch("{id}/desactivar")]
        public async Task<IActionResult> Desactivar(int id)
        {
            var r = await _svc.DesactivarAsync(id);
            return r.IsSuccess ? NoContent() : BadRequest(r.Error);
        }

        [HttpPatch("{id}/perfil")]
        public async Task<IActionResult> ActualizarPerfil(int id,
            [FromBody] ActualizarUsuarioRequest req)
        {
            var r = await _svc.ActualizarPerfilAsync(id, req);
            return r.IsSuccess ? NoContent() : BadRequest(r.Error);
        }
    }
}
