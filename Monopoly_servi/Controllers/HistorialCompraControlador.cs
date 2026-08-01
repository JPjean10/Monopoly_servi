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
    [Route("HistorialCompra")]
    public class HistorialCompraControlador : Controller
    {
        IHistorialCompraInterfaz _historialCompraService;
        private readonly IConfiguration _env;
        private readonly IHubContext<GameHub> _hubContext;

        public HistorialCompraControlador(IHistorialCompraInterfaz historialCompraService, IConfiguration config, IHubContext<GameHub> hubContext)
        {
            _historialCompraService = historialCompraService;
            _env = config;
            _hubContext = hubContext;
        }

        [HttpPost()]
        public async Task<Response2<bool>> InsertarHistorialCompra([FromBody] HistorialCompraModel historialCompra)
        {
            try
            {
                var outResp = await _historialCompraService.InsertarHistorialCompra(historialCompra);
                // Notificar a través del WebSocket que los datos del jugador han cambiado
                await _hubContext.Clients.All.SendAsync("actualizar_datos_partida", historialCompra.JugadorId);
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
        public async Task<Response2<List<HistorialCompraModel>>> ListarHistorialCompras()
        {
            try
            {
                var outResp = await _historialCompraService.ListarHistorialCompras();
                return new Response2<List<HistorialCompraModel>>(outResp);
            }
            catch (Exception ex)
            {
                return new Response2<List<HistorialCompraModel>>(ex);
            }
        }
    }
}
