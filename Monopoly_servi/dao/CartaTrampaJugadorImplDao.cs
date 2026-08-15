using Azure.Core;
using Microsoft.Data.SqlClient;
using Monopoly_servi.interfaz;
using Monopoly_servi.model;
using System.Data;

namespace Monopoly_servi.dao
{
    public class CartaTrampaJugadorImplDao : ICartaTrampaJugadorInterfaz
    {
        private readonly string _connectionString;
        private readonly IConfiguration _env;

        public CartaTrampaJugadorImplDao(IConfiguration config)
        {
            _env = config;
            _connectionString = config.GetConnectionString("DefaultConnection") ?? "";
        }

        public async Task<List<CartaTrampaJugadorModel>> ListarCartaTrampaJugador(int jugadorId)
        {
            var lista = new List<CartaTrampaJugadorModel>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_ListarCartaTrampaJugador", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@in_jugador_id", jugadorId);

            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lista.Add(new CartaTrampaJugadorModel
                {
                    CartaJugadorId = reader.GetInt32(reader.GetOrdinal("carta_jugador_id")),
                    JugadorId = reader.GetInt32(reader.GetOrdinal("jugador_id")),
                    CartaId = reader.GetInt32(reader.GetOrdinal("carta_id")),
                    CartaTrampaModel = new CartaTrampaModel
                    {
                        titulo = reader.GetString(reader.GetOrdinal("titulo")),
                        descripcion = reader.GetString(reader.GetOrdinal("descripcion")),
                        monto = reader.IsDBNull("monto") ? null : reader.GetInt32("monto"),
                        CodigoAccion = reader.GetString(reader.GetOrdinal("codigo_accion"))
                    }
                });
            }
            return lista;
        }

        public async Task<String> ProcesarCartaTrampa(CartaTrampaJugadorModel cartaTrampaJugadorModel) 
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_ProcesarCartaTrampa", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@in_jugador_id", cartaTrampaJugadorModel.JugadorId);
            cmd.Parameters.AddWithValue("@var_codigo_carta", cartaTrampaJugadorModel.CartaTrampaModel.CodigoAccion);

            await conn.OpenAsync();
            var resultado = await cmd.ExecuteScalarAsync();

            return resultado?.ToString();
        }

        public async Task<String> ProcesarCartaInventarioJugador(CartaTrampaJugadorModel cartaTrampaJugadorModel)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_ProcesarCartaInventarioJugador", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@in_carta_jugador_id", cartaTrampaJugadorModel.CartaJugadorId);
            cmd.Parameters.AddWithValue("@in_jugador_id", cartaTrampaJugadorModel.JugadorId);
            cmd.Parameters.AddWithValue("@var_codigo_carta", cartaTrampaJugadorModel.CartaTrampaModel.CodigoAccion);

            await conn.OpenAsync();
            var resultado = await cmd.ExecuteScalarAsync();

            return resultado?.ToString();
        }

    }
}
