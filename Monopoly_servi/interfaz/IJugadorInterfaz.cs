using Monopoly_servi.Models;
using MonopolyService.Models;

namespace Monopoly_servi.interfaz
{
    public interface IJugadorInterfaz
    {
        Task<String> InsertarJugador(JugadorModel jugador);
        Task<List<JugadorModel>> ListarJugadores();
        Task<String> EliminarJugador(int jugadorId);
        Task<List<JugadorModel>> ObtenerJugadorPorId(int jugadorId);
        Task<String> EjecutarAccionBanco(AccionBancoModel accionBanco);
        Task<List<AccionBancoModel>> ListarOpcionBanco();
    }
}
