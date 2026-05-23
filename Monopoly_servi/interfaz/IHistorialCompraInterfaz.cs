using Monopoly_servi.model;
using MonopolyService.Models;

namespace Monopoly_servi.interfaz
{
    public interface IHistorialCompraInterfaz
    {
        Task<Response2<bool>> InsertarHistorialCompra(HistorialCompraModel historialCompra);
        Task<Response2<List<HistorialCompraModel>>> ListarHistorialCompras();
    }
}
