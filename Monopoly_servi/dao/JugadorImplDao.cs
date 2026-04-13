using Microsoft.Data.SqlClient;
using Monopoly_servi.interfaz;
using Monopoly_servi.Models;
using MonopolyService.Models;
using System.Data;

namespace Monopoly_servi.dao
{
public class JugadorImplDao : IJugadorInterfaz
    {
        private readonly string _connectionString;
        private readonly IConfiguration _env;

        public JugadorImplDao(IConfiguration config)
        {
            _env = config;
            _connectionString = config.GetConnectionString("DefaultConnection") ?? "";
        }

        public async Task<Response2<bool>> InsertarJugador(JugadorModel jugador)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("sp_InsertarJugador", conn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@var_nombre", jugador.Nombre);

                await conn.OpenAsync();
                var resultado = await cmd.ExecuteScalarAsync();

                string mensaje = resultado?.ToString() ?? "Jugador insertado correctamente.";

                return new Response2<bool>(201, mensaje, true);
            }
            catch (SqlException ex)
            {
                // En C#, SqlState se maneja por Number o State
                return new Response2<bool>(ex, ex.Number);
            }
        }

        public async Task<Response2<List<JugadorModel>>> ListarJugadores()
        {
            try
            {
                var lista = new List<JugadorModel>();

                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("sp_ListarJugadores", conn);

                cmd.CommandType = CommandType.StoredProcedure;

                await conn.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    // Creamos el jugador con su tarjeta anidada para guardar el monto
                    lista.Add(new JugadorModel
                    {
                        JugadorId = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        EsBanco = reader.GetBoolean(2),
                        Tarjeta = new TarjetaModel
                        {
                            // reader.GetDouble(3) obtiene el monto de la tarjeta
                            Monto = Convert.ToDouble(reader["monto"])
                        }
                    });
                }
                return new Response2<List<JugadorModel>>(lista);
            }
            catch (Exception ex) { return new Response2<List<JugadorModel>>(ex); }
        }
    }
}
