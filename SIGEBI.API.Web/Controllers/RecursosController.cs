using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Interfaces;

namespace SIGEBI.API.Web.Controllers
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

        [HttpGet("disponibles")]
        public async Task<IActionResult> GetDisponibles()
        {
            var r = await _svc.ObtenerTodosAsync();
            var disponibles = r.Value!.Where(x => x.Disponible);
            return Ok(disponibles);
        }
    }
}
