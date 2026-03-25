using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.Interfaces;

namespace SIGEBI.API.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrestamosController : ControllerBase
    {
        private readonly IPrestamoAppService _svc;

        public PrestamosController(IPrestamoAppService svc) => _svc = svc;

        [HttpGet("usuario/{idUsuario}")]
        public async Task<IActionResult> GetPorUsuario(int idUsuario)
        {
            var r = await _svc.ObtenerTodosAsync();
            if (!r.IsSuccess) return BadRequest(r.Error);
            var prestamos = r.Value!.Where(p => p.IdUsuario == idUsuario);
            return Ok(prestamos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var r = await _svc.ObtenerPorIdAsync(id);
            return r.IsSuccess ? Ok(r.Value) : NotFound(r.Error);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearPrestamoRequest req)
        {
            var r = await _svc.CrearAsync(req);
            return r.IsSuccess ? Ok(r.Value) : BadRequest(r.Error);
        }

        [HttpPatch("{id}/devolver")]
        public async Task<IActionResult> Devolver(int id)
        {
            var r = await _svc.ProcesarDevolucionAsync(id);
            return r.IsSuccess ? NoContent() : BadRequest(r.Error);
        }
    }
}
