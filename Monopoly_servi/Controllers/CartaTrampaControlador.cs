using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Monopoly_servi.Hubs;
using Monopoly_servi.interfaz;
using Monopoly_servi.model;
using Monopoly_servi.Models;
using MonopolyService.Models;

namespace Monopoly_servi.Controllers
{
    [ApiController]
    [Route("CartaTrampa")]
    public class CartaTrampaControlador : Controller
    {
        private readonly ICartaTrampaInterfaz _cartaTrampaService;
        private readonly IConfiguration _env;
        private readonly IHubContext<GameHub> _hubContext;

        public CartaTrampaControlador(ICartaTrampaInterfaz cartaTrampaService, IConfiguration config, IHubContext<GameHub> hubContext)
        {
            _cartaTrampaService = cartaTrampaService;
            _env = config;
            _hubContext = hubContext;
        }

        [HttpGet()]
        public async Task<Response2<List<CartaTrampaModel>>> ListarCartasTrampa()
        {
            try
            {
                var outResp = await _cartaTrampaService.ListarCartaTrampa();
                return new Response2<List<CartaTrampaModel>>(outResp);
            }
            catch (Exception ex)
            {
                return new Response2<List<CartaTrampaModel>>(ex);
            }
        }
    }
}
