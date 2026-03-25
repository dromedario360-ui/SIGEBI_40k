using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.Interfaces;

namespace SIGEBI.API.Desktop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PenalizacionesController : ControllerBase
    {
        private readonly IPenalizacionAppService _svc;

        public PenalizacionesController(IPenalizacionAppService svc) => _svc = svc;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var r = await _svc.ObtenerTodasAsync();
            return Ok(r.Value);
        }

        [HttpGet("usuario/{idUsuario}")]
        public async Task<IActionResult> GetPorUsuario(int idUsuario)
        {
            var r = await _svc.ObtenerPorUsuarioAsync(idUsuario);
            return Ok(r.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearPenalizacionRequest req)
        {
            var r = await _svc.CrearAsync(req);
            return r.IsSuccess ? Ok(r.Value) : BadRequest(r.Error);
        }

        [HttpPatch("{id}/finalizar")]
        public async Task<IActionResult> Finalizar(int id)
        {
            var r = await _svc.FinalizarAsync(id);
            return r.IsSuccess ? NoContent() : BadRequest(r.Error);
        }
    }
}
