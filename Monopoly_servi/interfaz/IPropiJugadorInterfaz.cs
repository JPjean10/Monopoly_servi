using Monopoly_servi.model;
using MonopolyService.Models;

namespace Monopoly_servi.interfaz
{
    public interface IPropiJugadorInterfaz
    {
        Task<Response2<bool>> ComprarPropiedad(PropiJugadorModel propiJugador);
    }
}
