using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Monopoly_servi.Hubs;
using Monopoly_servi.interfaz;

namespace Monopoly_servi.Controllers
{
    [ApiController]
    [Route("Propiedad")]
    public class PropiedadControlador : Controller
    {
        private readonly IConfiguration _env;
        private readonly IHubContext<GameHub> _hubContext;
        private readonly IPropiedadInterfaz _propiedadService;

        public PropiedadControlador(IPropiedadInterfaz propiedadService, IConfiguration config, IHubContext<GameHub> hubContext)
        {
            _propiedadService = propiedadService;
            _env = config;
            _hubContext = hubContext;
        }

        [HttpGet("{jugadorId}")]
        public async Task<IActionResult> PropiededadXJugador(int jugadorId) {
            var response = await _propiedadService.PropiededadXJugador(jugadorId);
            return StatusCode(response.StatusCode, response);
        }
    }
}
