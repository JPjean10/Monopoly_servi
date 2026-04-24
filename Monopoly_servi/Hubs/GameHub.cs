using Microsoft.AspNetCore.SignalR;

namespace Monopoly_servi.Hubs
{
    public class GameHub : Hub
    {
        // Este método notificará a todos los clientes conectados
        public async Task NotifyPlayersUpdated()
        {
            await Clients.All.SendAsync("actulizar_lista_jugador");
        }
        public async Task StartGame()
        {
            await Clients.All.SendAsync("partida_iniciada");
        }
    }
}
