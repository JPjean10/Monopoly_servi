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
                using var cmd = new SqlCommand("sp_Comprar", conn);

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

        public async Task<Response2<List<PropiJugadorModel>>> AlquilertXJugador(int PropiedadJugadorId)
        {
            try
            {

                var lista = new List<PropiJugadorModel>();

                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("sp_AlquilertXJugador", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@in_jugador_id", PropiedadJugadorId);
                await conn.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    // Creamos el jugador con su tarjeta anidada para guardar el monto
                    lista.Add(new PropiJugadorModel
                    {
                        PropiedadJugadorId = reader.GetInt32(reader.GetOrdinal("propiedad_jugador_id")),
                        PropiedadId = reader.GetInt32(reader.GetOrdinal("propiedad_id")),
                        Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                        NivelActual = reader.GetInt32(reader.GetOrdinal("nivel_actual")),
                        Renta = reader.GetInt32(reader.GetOrdinal("renta")),
                        Propiedad = new PropiedadModel { 
                            Precio = reader.GetInt32(reader.GetOrdinal("precio")),
                        }
                    });
                }
                return new Response2<List<PropiJugadorModel>>(lista);
            }
            catch (Exception ex)
            {
                return new Response2<List<PropiJugadorModel>>(ex);
            }
        }
        public async Task<Response2<bool>> CobrarRenta(PropiJugadorModel propiJugador)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("sp_CobrarRenta", conn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PagadorId", propiJugador.JugadorId);
                cmd.Parameters.AddWithValue("@PropiedadId", propiJugador.PropiedadId);
                cmd.Parameters.AddWithValue("@Nivel", propiJugador.NivelActual);

                await conn.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    string message = reader.GetString(reader.GetOrdinal("Message"));
                    int cobradorId = reader.GetInt32(reader.GetOrdinal("CobradorRealId"));

                    // Concatenamos el mensaje limpio con el ID usando un separador '|'
                    // Esto produce por ejemplo: "Renta cobrada exitosamente.|2"
                    string dataCombinada = $"{message}|{cobradorId}";

                    return new Response2<bool>(201, dataCombinada, true);
                }

                return new Response2<bool>(500, "No se recibió respuesta de la base de datos.", false);
            }
            catch (SqlException ex)
            {
                // Esto captura los THROW 50000 que pusiste en el SQL
                return new Response2<bool>(ex, ex.Number);
            }
        }

        public async Task<Response2<int>> VenderPropiedadesMasivo(int jugadorId, string propiedadesIds)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("sp_HipotecarVenderPropiedades", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@JugadorId", jugadorId);
                cmd.Parameters.AddWithValue("@PropiedadesIds", propiedadesIds);

                await conn.OpenAsync();

                    return new Response2<int>(201, "hipoteca exotosa",true);
            }
            catch (SqlException ex)
            {
                return new Response2<int>(ex, ex.Number);
            }
        }
    }
}
