using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Monopoly_servi.Hubs;
using Monopoly_servi.interfaz;
using Monopoly_servi.model;
using MonopolyService.Models;

namespace Monopoly_servi.Controllers
{
    [ApiController]
    [Route("CartaTrampaJugador")]
    public class CartaTrampaJugadorControlador : Controller
    {
        private readonly ICartaTrampaJugadorInterfaz _cartaTrampaJugadorService;
        private readonly IConfiguration _env;
        private readonly IHubContext<GameHub> _hubContext;

        public CartaTrampaJugadorControlador(ICartaTrampaJugadorInterfaz cartaTrampaJugadorService, IConfiguration config, IHubContext<GameHub> hubContext)
        {
            _cartaTrampaJugadorService = cartaTrampaJugadorService;
            _env = config;
            _hubContext = hubContext;
        }

        [HttpGet("{jugadorId}")]
        public async Task<Response2<List<CartaTrampaJugadorModel>>> ListarCartasTrampaJugador(int jugadorId)
        {
            try
            {
                var outResp = await _cartaTrampaJugadorService.ListarCartaTrampaJugador(jugadorId);
                return new Response2<List<CartaTrampaJugadorModel>>(outResp);
            }
            catch (Exception ex)
            {
                return new Response2<List<CartaTrampaJugadorModel>>(ex);
            }
        }

        [HttpPost]
        public async Task<Response2<bool>> ProcesarCartaTrampa([FromBody] CartaTrampaJugadorModel request)
        {
            try
            {
                var outResp =  await _cartaTrampaJugadorService.ProcesarCartaTrampa(request);
                // Notificar a través del WebSocket que los datos del jugador han cambiado
                await _hubContext.Clients.All.SendAsync("actualizar_datos_partida", 0);
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

        [HttpDelete]
        public async Task<Response2<bool>> ProcesarCartaInventarioJugador([FromBody]  CartaTrampaJugadorModel cartaTrampaJugadorModel)
        {
            try
            {
                var outResp = await _cartaTrampaJugadorService.ProcesarCartaInventarioJugador(cartaTrampaJugadorModel);
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
