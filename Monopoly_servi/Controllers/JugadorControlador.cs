using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Monopoly_servi.Hubs;
using Monopoly_servi.interfaz;
using Monopoly_servi.Models;
using MonopolyService.Models;

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
        public async Task<Response2<bool>> InsertarJugador([FromBody] JugadorModel jugador)
        {
            try
            {
                var outResp = await _jugadorService.InsertarJugador(jugador);
                // Notificar a través del WebSocket que los datos del jugador han cambiado
                await _hubContext.Clients.All.SendAsync("actulizar_lista_jugador");
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

        [HttpGet()]
        public async Task<Response2<List<JugadorModel>>> ListarJugadores()
        {
            try
            {
                var outResp = await _jugadorService.ListarJugadores();
                return new Response2<List<JugadorModel>>(outResp);
            }
            catch (Exception ex)
            {
                return new Response2<List<JugadorModel>>(ex);
            }
        }

        [HttpDelete("{jugadorId}")]
        public async Task<Response2<bool>> EliminarJugador(int jugadorId)
        {

            try
            {
                var outResp = await _jugadorService.EliminarJugador(jugadorId);
                // Notificar a través del WebSocket que los datos del jugador han cambiado
                await _hubContext.Clients.All.SendAsync("actulizar_lista_jugador");
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
        
        [HttpGet("buscar/{jugadorId}")]
        public async Task<Response2<List<JugadorModel>>> ObtenerJugadorPorId(int jugadorId) 
        {
            try
            {
                var outResp = await _jugadorService.ObtenerJugadorPorId(jugadorId);
                return new Response2<List<JugadorModel>>(outResp);
            }
            catch (Exception ex)
            {
                return new Response2<List<JugadorModel>>(ex);
            }
        }
        
        [HttpPut]
        public async Task<Response2<bool>> EjecutarAccionBanco([FromBody] AccionBancoModel accionBanco)
        {
            try
            {
                String outResp = await _jugadorService.EjecutarAccionBanco(accionBanco);
                    // Notificar a través del WebSocket que los datos del jugador han cambiado
                    await _hubContext.Clients.All.SendAsync("actualizar_datos_partida", 0);
                return new Response2<bool>(200, outResp, true);
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
        
        [HttpGet("listar-opcion-banco")]
        public async Task<Response2<List<AccionBancoModel>>> ListarOpcionBanco() 
        {
            try
            {
                var outResp = await _jugadorService.ListarOpcionBanco();
                return new Response2<List<AccionBancoModel>>(outResp);
            }
            catch (Exception ex)
            {
                return new Response2<List<AccionBancoModel>>(ex);
            }
        }
    }
}