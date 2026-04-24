using Monopoly_servi.Models;
using MonopolyService.Models;

namespace Monopoly_servi.interfaz
{
    public interface IJugadorInterfaz
    {
        Task<Response2<bool>> InsertarJugador(JugadorModel jugador);
        Task <Response2<List<JugadorModel>>> ListarJugadores();
        Task<Response2<bool>> EliminarJugador(int jugadorId);
    }
}
