using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Monopoly_servi.Hubs;
using Monopoly_servi.interfaz;
using Monopoly_servi.model;
using Monopoly_servi.Models;
using MonopolyService.Models;

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
        public async Task<Response2<bool>> AdquirirOMejorarPropiedad([FromBody] PropiJugadorModel propiJugador)
        {
            try
            {
                var outResp = await _propiJugadorService.AdquirirOMejorarPropiedad(propiJugador);
                // Notificar a través del WebSocket que los datos del jugador han cambiado
                await _hubContext.Clients.All.SendAsync("actualizar_datos_partida", propiJugador.JugadorId);
                return new Response2<bool>(201, outResp, true);
            }
            catch (SqlException ex)
            {
                // AQUÍ pasamos ex.Number para que identifique el 50000 y asigne 401
                return new Response2<bool>(ex, ex.Number);
            }
            catch (Exception ex)
            {
                return new Response2<bool>(ex);
            }
        }
        [HttpGet("{jugadorId}")]
        public async Task<Response2<List<PropiJugadorModel>>> AlquilertXJugador(int jugadorId)
        {

            try
            {
                var outResp = await _propiJugadorService.AlquilertXJugador(jugadorId);
                return new Response2<List<PropiJugadorModel>>(outResp);
            }
            catch (Exception ex)
            {
                return new Response2<List<PropiJugadorModel>>(ex);
            }
        }

        [HttpPost("cobrar-renta")]
        public async Task<Response2<bool>> CobrarRenta([FromBody] PropiJugadorModel propiJugador)
        {

            try
            {
                var outResp = await _propiJugadorService.CobrarRenta(propiJugador);

                if (!string.IsNullOrEmpty(outResp))
                {
                    // Limpiamos el pipe '|' si la base de datos lo devolvió para no romper el mensaje de la UI
                    if (outResp.Contains("|"))
                    {
                        outResp = outResp.Split('|')[0];
                    }

                    // Emitimos un único aviso global (0) a todos los conectados
                    await _hubContext.Clients.All.SendAsync("actualizar_datos_partida", 0);
                }
                return new Response2<bool>(201, outResp, true);
            }
            catch (SqlException ex)
            {
                // AQUÍ pasamos ex.Number para que identifique el 50000 y asigne 401
                return new Response2<bool>(ex, ex.Number);
            }
            catch (Exception ex)
            {
                return new Response2<bool>(ex);
            }
        }

        [HttpPost("vender-propiedades")]
        public async Task<Response2<bool>> VenderPropiedades([FromBody] VentaMasivaRequest request)
        {
            try
            {
                await _propiJugadorService.VenderPropiedadesMasivo(request);
                // Notificar a través del WebSocket que los datos del jugador han cambiado
                await _hubContext.Clients.All.SendAsync("actualizar_datos_partida", request.JugadorId);
                return new Response2<bool>(201, "hipoteca exotosa", true);
            }
            catch (SqlException ex)
            {
                // AQUÍ pasamos ex.Number para que identifique el 50000 y asigne 401
                return new Response2<bool>(ex, ex.Number);
            }
            catch (Exception ex)
            {
                return new Response2<bool>(ex);
            }
        }
        [HttpPost("subasta-propiedad")]
        public async Task<Response2<bool>> ProcesarSubasta([FromBody] PropiJugadorModel propiJugador)
        {
            try
            {
                var outResp = await _propiJugadorService.ProcesarSubasta(propiJugador);
                // Notificar a través del WebSocket que los datos del jugador han cambiado
                await _hubContext.Clients.All.SendAsync("actualizar_datos_partida", propiJugador.JugadorId);
                return new Response2<bool>(201, outResp, true);
            }
            catch (SqlException ex)
            {
                // AQUÍ pasamos ex.Number para que identifique el 50000 y asigne 401
                return new Response2<bool>(ex, ex.Number);
            }
            catch (Exception ex)
            {
                return new Response2<bool>(ex);
            }
        }
    }
}
