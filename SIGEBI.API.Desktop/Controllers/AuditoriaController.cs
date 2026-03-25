using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Interfaces;

namespace SIGEBI.API.Desktop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditoriaController : ControllerBase
    {
        private readonly IAuditoriaAppService _svc;

        public AuditoriaController(IAuditoriaAppService svc) => _svc = svc;

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

        [HttpGet("rango")]
        public async Task<IActionResult> GetPorFecha(
            [FromQuery] DateTime desde,
            [FromQuery] DateTime hasta)
        {
            var r = await _svc.ObtenerPorFechaAsync(desde, hasta);
            return Ok(r.Value);
        }
    }
}
