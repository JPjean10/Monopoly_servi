using Microsoft.Data.SqlClient;
using Monopoly_servi.interfaz;
using Monopoly_servi.Models;
using MonopolyService.Models;
using System.Collections;
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

        public async Task<String> InsertarJugador(JugadorModel jugador)
        {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("sp_InsertarJugador", conn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@var_nombre", jugador.Nombre);

                await conn.OpenAsync();
                var resultado = await cmd.ExecuteScalarAsync();

               return resultado?.ToString() ?? "Jugador insertado correctamente.";
        }

        public async Task<List<JugadorModel>> ListarJugadores()
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
                        JugadorId = reader.GetInt32(reader.GetOrdinal("jugador_id")),
                        Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                        EsBanco = reader.GetBoolean(reader.GetOrdinal("es_banco")),
                        Tarjeta = new TarjetaModel
                        {
                            // reader.GetDouble(3) obtiene el monto de la tarjeta
                            Monto = reader.GetInt32(reader.GetOrdinal("monto")),
                        }
                    });
                }
            return lista;
        }

        public async Task<String> EliminarJugador(int jugadorId)
        {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("sp_EliminarJugador", conn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@in_jugador_id", jugadorId);

                await conn.OpenAsync();
                await cmd.ExecuteScalarAsync();

                return "Jugador eliminado correctamente.";
        }

        public async Task<List<JugadorModel>> ObtenerJugadorPorId(int jugadorId)
        {
                var lista = new List<JugadorModel>();

                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("sp_ObtenerDetalleJugador", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@in_jugador_id", jugadorId);

                await conn.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    // Creamos el jugador con su tarjeta anidada para guardar el monto
                    lista.Add(new JugadorModel
                    {
                        JugadorId = reader.GetInt32(reader.GetOrdinal("jugador_id")),
                        Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                        EsBanco = reader.GetBoolean(reader.GetOrdinal("es_banco")),
                        Tarjeta = new TarjetaModel
                        {
                            // reader.GetDouble(3) obtiene el monto de la tarjeta
                            Monto = reader.GetInt32(reader.GetOrdinal("monto")),
                        }
                    });
                }
                return lista;
        }

        public async Task<String> EjecutarAccionBanco(AccionBancoModel accionBanco)
        {
                var lista = new List<JugadorModel>();

                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("sp_EjecutarAccionBanco", conn);

                cmd.CommandType = CommandType.StoredProcedure; 
                cmd.Parameters.AddWithValue("@in_jugador_id", accionBanco.JugadorId);
                cmd.Parameters.AddWithValue("@in_opcion_banco_id", accionBanco.OpcionBancoId);

                await conn.OpenAsync();
                var resultado = await cmd.ExecuteScalarAsync();

                return resultado?.ToString() ?? "Acción procesada correctamente.";
        }

        public async Task<List<AccionBancoModel>> ListarOpcionBanco()
        {
                var lista = new List<AccionBancoModel>();

                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("sp_ListarOpcionBanco", conn);

                cmd.CommandType = CommandType.StoredProcedure;

                await conn.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    lista.Add(new AccionBancoModel
                    {
                        OpcionBancoId = reader.GetInt32(reader.GetOrdinal("opcion_banco_id")),
                        Titulo = reader.GetString(reader.GetOrdinal("titulo"))
                    });
                }
                return lista;
            }
        }

}
