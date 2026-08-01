using Monopoly_servi.model;
using MonopolyService.Models;

namespace Monopoly_servi.interfaz
{
    public interface IHistorialCompraInterfaz
    {
        Task<String> InsertarHistorialCompra(HistorialCompraModel historialCompra);
        Task<List<HistorialCompraModel>> ListarHistorialCompras();
    }
}
