using Microsoft.Data.SqlClient;
using Monopoly_servi.interfaz;
using Monopoly_servi.model;
using Monopoly_servi.Models;
using MonopolyService.Models;
using System.Data;

namespace Monopoly_servi.dao
{
    public class PropiedadImplDao : IPropiedadInterfaz
    {
        private readonly string _connectionString;
        private readonly IConfiguration _env;

        public PropiedadImplDao(IConfiguration config)
        {
            _env = config;
            _connectionString = config.GetConnectionString("DefaultConnection") ?? "";
        }

        public async Task<List<PropiedadModel>> PropiededadXJugador(int PropiedadJugadorId, int descuento)
        {
                var lista = new List<PropiedadModel>();

                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("sp_PropiededadXJugador", conn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@in_jugador_id", PropiedadJugadorId);
            cmd.Parameters.AddWithValue("@in_descuento", descuento);

                await conn.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync()) 
                {
                    lista.Add(new PropiedadModel() 
                    {
                        PropiedadId = reader.GetInt32(reader.GetOrdinal("propiedad_id")),
                        Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                        Precio = reader.GetInt32(reader.GetOrdinal("precio")),
                        precio_descuento = reader.GetInt32(reader.GetOrdinal("precio_descuento")),
                        Direccion = reader.GetString(reader.GetOrdinal("direccion"))
                    });
                }
                return lista;
        }


    }
}
