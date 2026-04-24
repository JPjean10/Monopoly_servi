using Monopoly_servi.model;
using MonopolyService.Models;

namespace Monopoly_servi.interfaz
{
    public interface IPropiedadInterfaz
    {
        Task<Response2<List<PropiedadModel>>> ListarPropiedades();
    }
}
