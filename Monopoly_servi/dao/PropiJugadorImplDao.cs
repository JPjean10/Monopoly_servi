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

        public async Task<String> ComprarPropiedad(PropiJugadorModel propiJugador)
        {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("sp_Comprar", conn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@in_jugador_id", propiJugador.JugadorId);
                cmd.Parameters.AddWithValue("@in_propiedad_id", propiJugador.PropiedadId);

                await conn.OpenAsync();
                var resultado = await cmd.ExecuteScalarAsync();

                return resultado?.ToString() ?? "Compra Exitosa.";
        }

        public async Task<List<PropiJugadorModel>> AlquilertXJugador(int PropiedadJugadorId)
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
                return lista;
        }
        public async Task<String> CobrarRenta(PropiJugadorModel propiJugador)
        {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("sp_CobrarRenta", conn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PagadorId", propiJugador.JugadorId);
                cmd.Parameters.AddWithValue("@PropiedadId", propiJugador.PropiedadId);
                cmd.Parameters.AddWithValue("@Nivel", propiJugador.NivelActual);

                await conn.OpenAsync();
                var resultado = await cmd.ExecuteScalarAsync();

                return resultado?.ToString();
        }

        public async Task VenderPropiedadesMasivo(VentaMasivaRequest request)
        {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("sp_HipotecarPropiedades", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@JugadorId", request.JugadorId);
                cmd.Parameters.AddWithValue("@PropiedadesIds", request.PropiedadesIds);

                await conn.OpenAsync();
                var resultado = await cmd.ExecuteScalarAsync();
        }
    }
}
