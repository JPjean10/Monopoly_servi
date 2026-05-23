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
