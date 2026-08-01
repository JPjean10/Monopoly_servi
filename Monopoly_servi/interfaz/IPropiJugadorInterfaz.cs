using Monopoly_servi.model;
using MonopolyService.Models;

namespace Monopoly_servi.interfaz
{
    public interface IPropiJugadorInterfaz
    {
        Task<String> ComprarPropiedad(PropiJugadorModel propiJugador);
        Task<List<PropiJugadorModel>> AlquilertXJugador(int PropiedadJugadorId);
        Task<String> CobrarRenta(PropiJugadorModel propiJugador);
        Task VenderPropiedadesMasivo(VentaMasivaRequest request);

    }
}
