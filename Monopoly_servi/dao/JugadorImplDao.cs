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

                return new Response2<bool>(201, resultado?.ToString() ?? "Jugador insertado correctamente.", true);
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
                return new Response2<List<JugadorModel>>(lista);
            }
            catch (Exception ex) { return new Response2<List<JugadorModel>>(ex); 
            }
        }

        public async Task<Response2<bool>> EliminarJugador(int jugadorId)
        {
            try 
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("sp_EliminarJugador", conn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@in_jugador_id", jugadorId);

                await conn.OpenAsync();
                await cmd.ExecuteScalarAsync();

                return new Response2<bool>(200, "jugador eliminado exitosamente", true);
            }
            catch (SqlException ex)
            {
                return new Response2<bool>(ex, ex.Number);
            }
        }

        public async Task<Response2<List<JugadorModel>>> ObtenerJugadorPorId(int jugadorId)
        {
            try
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
                return new Response2<List<JugadorModel>>(lista);
            }
            catch (Exception ex)
            {
                return new Response2<List<JugadorModel>>(ex);
            }
        }

        public async Task<Response2<bool>> EjecutarAccionBanco(AccionBancoModel accionBanco)
        {
            try
            {
                var lista = new List<JugadorModel>();

                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("sp_EjecutarAccionBanco", conn);

                cmd.CommandType = CommandType.StoredProcedure; 
                cmd.Parameters.AddWithValue("@in_jugador_id", accionBanco.JugadorId);
                cmd.Parameters.AddWithValue("@in_opcion_banco_id", accionBanco.OpcionBancoId);

                await conn.OpenAsync();
                var resultado = await cmd.ExecuteScalarAsync();

                return new Response2<bool>(200, resultado?.ToString() ?? "Acción procesada correctamente.", true);
            }
            catch (Exception ex)
            {
                return new Response2<bool>(ex);
            }
        }

        public async Task<Response2<List<AccionBancoModel>>> ListarOpcionBanco()
        {
            try {
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
                return new Response2<List<AccionBancoModel>>(lista);
            }
            catch (Exception ex)
            {
                return new Response2<List<AccionBancoModel>>(ex);
            }
        }
    }
}
