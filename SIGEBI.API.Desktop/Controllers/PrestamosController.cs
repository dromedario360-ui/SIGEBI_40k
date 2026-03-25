using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.Interfaces;

namespace SIGEBI.API.Desktop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrestamosController : ControllerBase
    {
        private readonly IPrestamoAppService _svc;

        public PrestamosController(IPrestamoAppService svc) => _svc = svc;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var r = await _svc.ObtenerTodosAsync();
            return Ok(r.Value);
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

        [HttpPost("marcar-vencidos")]
        public async Task<IActionResult> MarcarVencidos()
        {
            var r = await _svc.MarcarVencidosAsync();
            return r.IsSuccess ? NoContent() : BadRequest(r.Error);
        }
    }
}
