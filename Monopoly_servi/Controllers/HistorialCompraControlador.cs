using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Monopoly_servi.Hubs;
using Monopoly_servi.interfaz;
using Monopoly_servi.model;

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
        public async Task<IActionResult> InsertarHistorialCompra([FromBody] HistorialCompraModel historialCompra)
        {
            var outResp = await _historialCompraService.InsertarHistorialCompra(historialCompra);
            if (outResp.StatusCode == 201)
            {
                // Notificamos que los datos de la partida han cambiado, enviando el ID del comprador
                await _hubContext.Clients.All.SendAsync("actualizar_datos_partida", historialCompra.JugadorId);
            }
            return StatusCode(outResp.StatusCode, outResp);
        }
        [HttpGet()]
        public async Task<IActionResult> ListarHistorialCompras()
        {
            var response = await _historialCompraService.ListarHistorialCompras();
            return StatusCode(response.StatusCode, response);
        }
    }
}
