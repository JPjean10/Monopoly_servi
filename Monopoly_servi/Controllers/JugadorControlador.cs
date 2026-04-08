using Microsoft.AspNetCore.Mvc;
using Monopoly_servi.interfaz;
using Monopoly_servi.Models;

namespace Monopoly_servi.Controllers
{
    [ApiController]
    [Route("Juagdor")]
    public class JugadorControlador : Controller
    {
        private readonly IJugadorInterfaz _jugadorService;
        private readonly IConfiguration _env;

        public JugadorControlador(IJugadorInterfaz jugadorService, IConfiguration config)
        {
            _jugadorService = jugadorService;
            _env = config;
        }

        [HttpPost("insert")]
        public async Task<IActionResult> InsertarJugador([FromBody] JugadorModel jugador)
        {
            var outResp = await _jugadorService.InsertarJugador(jugador);
            return StatusCode(outResp.StatusCode, outResp);
        }
    }
}
