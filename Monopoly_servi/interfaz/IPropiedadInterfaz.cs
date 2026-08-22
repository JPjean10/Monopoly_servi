using Monopoly_servi.model;
using MonopolyService.Models;

namespace Monopoly_servi.interfaz
{
    public interface IPropiedadInterfaz
    {
        Task<List<PropiedadModel>> PropiededadXJugador(int PropiedadJugadorId, int descuento);
    }
}
