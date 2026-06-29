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
        [HttpGet("{jugadorId}")]
        public async Task<IActionResult> AlquilertXJugador(int jugadorId)
        {
            var response = await _propiJugadorService.AlquilertXJugador(jugadorId);
            // Retornamos el StatusCode interno (ej. 200 o 500)
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("CobrarRenta")]
        public async Task<IActionResult> CobrarRenta([FromBody] PropiJugadorModel propiJugador)
        {
            var outResp = await _propiJugadorService.CobrarRenta(propiJugador);

            if (outResp.StatusCode == 201 && !string.IsNullOrEmpty(outResp.UserMssg))
            {
                // Limpiamos el pipe '|' si la base de datos lo devolvió para no romper el mensaje de la UI
                if (outResp.UserMssg.Contains("|"))
                {
                    outResp.UserMssg = outResp.UserMssg.Split('|')[0];
                }

                // Emitimos un único aviso global (0) a todos los conectados
                await _hubContext.Clients.All.SendAsync("actualizar_datos_partida", 0);
            }

            return StatusCode(outResp.StatusCode, outResp);
        }

        public class VentaMasivaRequest
        {
            public int JugadorId { get; set; }
            public string PropiedadesIds { get; set; } = string.Empty; // Cadena tipo "3,5,8"
        }

        [HttpPost("VenderPropiedades")]
        public async Task<IActionResult> VenderPropiedades([FromBody] VentaMasivaRequest request)
        {
            var outResp = await _propiJugadorService.VenderPropiedadesMasivo(request.JugadorId, request.PropiedadesIds);

            if (outResp.StatusCode == 201)
            {
                // Forzamos la actualización de saldos y pertenencias para todos los clientes en la partida
                await _hubContext.Clients.All.SendAsync("actualizar_datos_partida", request.JugadorId);
            }

            return StatusCode(outResp.StatusCode, outResp);
        }
    }
}
