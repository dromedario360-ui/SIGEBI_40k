using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.Interfaces;

namespace SIGEBI.API.Desktop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecursosController : ControllerBase
    {
        private readonly IRecursoAppService _svc;

        public RecursosController(IRecursoAppService svc) => _svc = svc;

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

        [HttpPost("libro")]
        public async Task<IActionResult> CrearLibro([FromBody] CrearLibroRequest req)
        {
            var r = await _svc.CrearLibroAsync(req);
            return r.IsSuccess ? Ok(r.Value) : BadRequest(r.Error);
        }

        [HttpPost("revista")]
        public async Task<IActionResult> CrearRevista([FromBody] CrearRevistaRequest req)
        {
            var r = await _svc.CrearRevistaAsync(req);
            return r.IsSuccess ? Ok(r.Value) : BadRequest(r.Error);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id,
            [FromBody] ActualizarRecursoRequest req)
        {
            var r = await _svc.ActualizarAsync(id, req);
            return r.IsSuccess ? Ok(r.Value) : BadRequest(r.Error);
        }

        [HttpPatch("{id}/incrementar")]
        public async Task<IActionResult> IncrementarStock(int id,
            [FromBody] AjustarStockRequest req)
        {
            var r = await _svc.AjustarStockAsync(id, req, true);
            return r.IsSuccess ? NoContent() : BadRequest(r.Error);
        }

        [HttpPatch("{id}/reducir")]
        public async Task<IActionResult> ReducirStock(int id,
            [FromBody] AjustarStockRequest req)
        {
            var r = await _svc.AjustarStockAsync(id, req, false);
            return r.IsSuccess ? NoContent() : BadRequest(r.Error);
        }
    }
}
