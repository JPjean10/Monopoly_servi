using Microsoft.AspNetCore.Mvc;
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

        public JugadorControlador(IJugadorInterfaz jugadorService, IConfiguration config)
        {
            _jugadorService = jugadorService;
            _env = config;
        }

        [HttpPost()]
        public async Task<IActionResult> InsertarJugador([FromBody] JugadorModel jugador)
        {
            var outResp = await _jugadorService.InsertarJugador(jugador);
            return StatusCode(outResp.StatusCode, outResp);
        }

        [HttpGet()]
        public async Task<IActionResult> ListarJugadores()
        {
            var response = await _jugadorService.ListarJugadores();

            // Retornamos el StatusCode interno (ej. 200 o 500)
            return StatusCode(response.StatusCode, response);
        }
    }
}
