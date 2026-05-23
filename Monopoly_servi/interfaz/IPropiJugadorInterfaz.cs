using Monopoly_servi.model;
using MonopolyService.Models;

namespace Monopoly_servi.interfaz
{
    public interface IPropiJugadorInterfaz
    {
        Task<Response2<bool>> ComprarPropiedad(PropiJugadorModel propiJugador);
        Task<Response2<List<PropiJugadorModel>>> AlquilertXJugador(int PropiedadJugadorId);
        Task<Response2<bool>> CobrarRenta(PropiJugadorModel propiJugador);
        Task<Response2<int>> VenderPropiedadesMasivo(int jugadorId, string propiedadesIds);

    }
}
