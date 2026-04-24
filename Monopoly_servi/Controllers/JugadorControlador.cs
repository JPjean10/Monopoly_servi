using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Monopoly_servi.Hubs;
using Monopoly_servi.interfaz;
using Monopoly_servi.Models;

namespace Monopoly_servi.Controllers
{
    [ApiController]
    [Route("Jugador")]
    public class JugadorControlador : Controller
    {
        private readonly IJugadorInterfaz _jugadorService;
        private readonly IConfiguration _env;
        private readonly IHubContext<GameHub> _hubContext;

        public JugadorControlador(IJugadorInterfaz jugadorService, IConfiguration config, IHubContext<GameHub> hubContext)
        {
            _jugadorService = jugadorService;
            _env = config;
            _hubContext = hubContext;
        }

        [HttpPost()]
        public async Task<IActionResult> InsertarJugador([FromBody] JugadorModel jugador)
        {
            var outResp = await _jugadorService.InsertarJugador(jugador);
            if (outResp.StatusCode == 201)
            {
                // Notificar a través del WebSocket que hay un nuevo jugador
                await _hubContext.Clients.All.SendAsync("actulizar_lista_jugador");
            }
            return StatusCode(outResp.StatusCode, outResp);
        }

        [HttpGet()]
        public async Task<IActionResult> ListarJugadores()
        {
            var response = await _jugadorService.ListarJugadores();

            // Retornamos el StatusCode interno (ej. 200 o 500)
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{jugadorId}")]
        public async Task<IActionResult> EliminarJugador(int jugadorId)
        {
            var outResp = await _jugadorService.EliminarJugador(jugadorId);

            if (outResp.StatusCode == 200)
            {
                // NOTIFICAMOS A TODOS que la lista cambió (alguien se fue o el banco cambió)
                await _hubContext.Clients.All.SendAsync("actulizar_lista_jugador");
            }

            return StatusCode(outResp.StatusCode, outResp);
         }
    }
}
