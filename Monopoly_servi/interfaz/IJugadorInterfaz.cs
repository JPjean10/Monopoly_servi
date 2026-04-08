using Monopoly_servi.Models;
using MonopolyService.Models;

namespace Monopoly_servi.interfaz
{
    public interface IJugadorInterfaz
    {
        Task<Response2<bool>> InsertarJugador(JugadorModel jugador);
    }
}
