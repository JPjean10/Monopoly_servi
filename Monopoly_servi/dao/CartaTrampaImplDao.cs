using Microsoft.Data.SqlClient;
using Monopoly_servi.interfaz;
using Monopoly_servi.model;
using System.Data;

namespace Monopoly_servi.dao
{
    public class CartaTrampaImplDao : ICartaTrampaInterfaz
    {
        private readonly string _connectionString;
        private readonly IConfiguration _env;
         public CartaTrampaImplDao(IConfiguration config)
        {
            _env = config;
            _connectionString = config.GetConnectionString("DefaultConnection") ?? "";
        }

        public async Task<List<CartaTrampaModel>> ListarCartaTrampa()
        {
            var lista = new List<CartaTrampaModel>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_ListarCartaTrampa", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(new CartaTrampaModel
                {
                    carta_id = reader.GetInt32(reader.GetOrdinal("carta_id")),
                    titulo = reader.GetString(reader.GetOrdinal("titulo")),
                    descripcion = reader.GetString(reader.GetOrdinal("descripcion")),
                    monto = reader.IsDBNull("monto") ? null : reader.GetInt32("monto"),
                    CodigoAccion = reader.GetString(reader.GetOrdinal("codigo_accion")),
                    peso = reader.GetInt32(reader.GetOrdinal("peso"))
                });
            }
            return lista;
        }
         
    }
}
