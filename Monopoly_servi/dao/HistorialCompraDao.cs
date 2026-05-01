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

        public async Task<Response2<bool>> InsertarHistorialCompra(HistorialCompraModel historialCompra)
        {
            try
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

                return new Response2<bool>(201, resultado?.ToString() ?? "historial insertado correctamente.", true);
            }
            catch (SqlException ex)
            {
                // En C#, SqlState se maneja por Number o State
                return new Response2<bool>(ex, ex.Number);
            }
        }
    }
}
