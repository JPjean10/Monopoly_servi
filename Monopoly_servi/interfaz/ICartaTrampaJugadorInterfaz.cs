using Monopoly_servi.model;

namespace Monopoly_servi.interfaz
{
    public interface ICartaTrampaJugadorInterfaz
    {
        Task<List<CartaTrampaJugadorModel>> ListarCartaTrampaJugador(int jugadorId);
        Task<String> ProcesarCartaTrampa(CartaTrampaJugadorModel cartaTrampaJugadorModel);
        Task<String> ProcesarCartaInventarioJugador(CartaTrampaJugadorModel cartaTrampaJugadorModel);
    }
}
