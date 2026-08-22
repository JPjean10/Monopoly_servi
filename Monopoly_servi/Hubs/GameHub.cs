using Microsoft.AspNetCore.SignalR;

namespace Monopoly_servi.Hubs
{
    public class GameHub : Hub
    {
        // Este método notificará a todos los clientes conectados
        public async Task NotificarJugadoresActualizados()
        {
            await Clients.All.SendAsync("actulizar_lista_jugador");
        }
        public async Task IniciarJuego()
        {
            await Clients.All.SendAsync("partida_iniciada");
        }
        public async Task EnviarSolicitudCompra(int jugadorId, int propiedadId, string nombreJugador, string mensajeSolicitud,int descuento)
        {
            // Notifica a todos (o solo al Banco si tienes grupos) que hay una nueva solicitud
            await Clients.All.SendAsync("nueva_solicitud_compra", jugadorId, propiedadId, nombreJugador, mensajeSolicitud, descuento);
        }
        public async Task NotificarDatosPartidaActualizados(int jugadorId)
        {
            // Envía el ID del jugador afectado para que los clientes sepan a quién refrescar
            await Clients.All.SendAsync("actualizar_datos_partida", jugadorId);
        }
    }
}
 