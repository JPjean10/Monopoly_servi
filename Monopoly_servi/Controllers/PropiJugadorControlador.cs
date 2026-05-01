using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Monopoly_servi.Hubs;
using Monopoly_servi.interfaz;
using Monopoly_servi.model;

namespace Monopoly_servi.Controllers
{
    [ApiController]
    [Route("PropiJugador")]
    public class PropiJugadorControlador : Controller
    {
        private readonly IConfiguration _env;
        private readonly IHubContext<GameHub> _hubContext;
        private readonly IPropiJugadorInterfaz _propiJugadorService;

        public PropiJugadorControlador(IPropiJugadorInterfaz propiJugadorService, IConfiguration config, IHubContext<GameHub> hubContext)
        {
            _propiJugadorService = propiJugadorService;
            _env = config;
            _hubContext = hubContext;
        }

        [HttpPost()]
        public async Task<IActionResult> ComprarPropiedad([FromBody] PropiJugadorModel propiJugador)
        {
            var outResp = await _propiJugadorService.ComprarPropiedad(propiJugador);
            if (outResp.StatusCode == 201)
            {
                // Notificamos que los datos de la partida han cambiado, enviando el ID del comprador
                await _hubContext.Clients.All.SendAsync("actualizar_datos_partida", propiJugador.JugadorId);
            }
            return StatusCode(outResp.StatusCode, outResp);
        }
    }
}
