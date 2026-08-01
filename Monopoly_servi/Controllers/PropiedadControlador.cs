using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Monopoly_servi.Hubs;
using Monopoly_servi.interfaz;
using Monopoly_servi.model;
using Monopoly_servi.Models;
using MonopolyService.Models;

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
        public async Task<Response2<List<PropiedadModel>>> PropiededadXJugador(int jugadorId) 
        {
            try
            {
                var outResp = await _propiedadService.PropiededadXJugador(jugadorId);
                return new Response2<List<PropiedadModel>>(outResp);
            }
            catch (Exception ex)
            {
                return new Response2<List<PropiedadModel>>(ex);
            }
        }
    }
}
