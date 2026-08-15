using Monopoly_servi.model;
using MonopolyService.Models;

namespace Monopoly_servi.interfaz
{
    public interface IPropiJugadorInterfaz
    {
        Task<String> AdquirirOMejorarPropiedad(PropiJugadorModel propiJugador);
        Task<List<PropiJugadorModel>> AlquilertXJugador(int PropiedadJugadorId);
        Task<String> CobrarRenta(PropiJugadorModel propiJugador);
        Task VenderPropiedadesMasivo(VentaMasivaRequest request);
        Task<String> ProcesarSubasta(PropiJugadorModel propiJugador);
    }
}
