using Microsoft.Data.SqlClient;
using Monopoly_servi.interfaz;
using Monopoly_servi.model;
using MonopolyService.Models;
using System.Data;

namespace Monopoly_servi.dao
{
    public class HistorialCompraDao : IHistorialCompraInterfaz
    {
        private readonly string _connectionString;
        private readonly IConfiguration _env;

        public HistorialCompraDao(IConfiguration config)
        {
            _env = config;
            _connectionString = config.GetConnectionString("DefaultConnection") ?? "";
        }

        public async Task<String> InsertarHistorialCompra(HistorialCompraModel historialCompra)
        {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("sp_InsertarHistorialCompra", conn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@in_jugador_id", historialCompra.JugadorId);
                cmd.Parameters.AddWithValue("@in_propiedad_id", historialCompra.PropiedadId);
                cmd.Parameters.AddWithValue("@var_tipo_compra", historialCompra.TipoCompra);
                cmd.Parameters.AddWithValue("@var_estado", historialCompra.Estado);

                await conn.OpenAsync();
                var resultado = await cmd.ExecuteScalarAsync();

                return resultado?.ToString() ?? "historial insertado correctamente.";
        }

        public async Task<List<HistorialCompraModel>> ListarHistorialCompras()
        {
                var lista = new List<HistorialCompraModel>();

                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("sp_ListarHistorialCompra", conn);

                cmd.CommandType = CommandType.StoredProcedure;

                await conn.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    // Creamos el jugador con su tarjeta anidada para guardar el monto
                    lista.Add(new HistorialCompraModel
                    {
                        TipoCompra = reader["tipo_compra"].ToString() ?? "",
                        Estado = reader["estado"].ToString() ?? "",
                        NombreJugador = reader["nombre_jugador"].ToString() ?? "",
                        NombrePropiedad = reader["nombre_propiedad"].ToString() ?? "",
                        mensage = reader["mensage"].ToString() ?? ""
                    });
                }
                return lista;
        }
    }
}
