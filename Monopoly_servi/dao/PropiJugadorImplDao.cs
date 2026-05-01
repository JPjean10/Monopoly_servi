using Microsoft.Data.SqlClient;
using Monopoly_servi.interfaz;
using Monopoly_servi.model;
using MonopolyService.Models;
using System.Data;

namespace Monopoly_servi.dao
{
    public class PropiJugadorImplDao : IPropiJugadorInterfaz
    {
        private readonly string _connectionString;
        private readonly IConfiguration _env;

        public PropiJugadorImplDao(IConfiguration config)
        {
            _env = config;
            _connectionString = config.GetConnectionString("DefaultConnection") ?? "";
        }

        public async Task<Response2<bool>> ComprarPropiedad(PropiJugadorModel propiJugador)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("sp_ComprarPropiedad", conn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@in_jugador_id", propiJugador.JugadorId);
                cmd.Parameters.AddWithValue("@in_propiedad_id", propiJugador.PropiedadId);

                await conn.OpenAsync();
                var resultado = await cmd.ExecuteScalarAsync();

                return new Response2<bool>(201, resultado?.ToString() ?? "Compra Exitosa.", true);
            }
            catch (SqlException ex)
            {
                // En C#, SqlState se maneja por Number o State
                return new Response2<bool>(ex, ex.Number);
            }
        }
    }
}
