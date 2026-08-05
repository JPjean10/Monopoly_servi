using Monopoly_servi.model;

namespace Monopoly_servi.interfaz
{
    public interface ICartaTrampaInterfaz
    {
        Task<List<CartaTrampaModel>> ListarCartaTrampa();
    }
}
