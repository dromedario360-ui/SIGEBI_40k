using Microsoft.AspNetCore.Mvc;
using SIGEBI.Application.Interfaces;

namespace SIGEBI.API.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PenalizacionesController : ControllerBase
    {
        private readonly IPenalizacionAppService _svc;
        public PenalizacionesController(IPenalizacionAppService svc) => _svc = svc;

        [HttpGet("usuario/{idUsuario}")]
        public async Task<IActionResult> GetPorUsuario(int idUsuario)
        {
            var r = await _svc.ObtenerPorUsuarioAsync(idUsuario);
            return r.IsSuccess ? Ok(r.Value) : BadRequest(r.Error);
        }
    }
}