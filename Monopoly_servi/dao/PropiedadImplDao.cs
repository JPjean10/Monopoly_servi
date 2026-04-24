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

        public async Task<Response2<List<PropiedadModel>>> ListarPropiedades()
        {
            try { 
                var lista = new List<PropiedadModel>();

                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("sp_ListarPropiedad", conn);

                cmd.CommandType = CommandType.StoredProcedure;

                await conn.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync()) 
                {
                    lista.Add(new PropiedadModel() 
                    {
                        PropiedadId = reader.GetInt32(reader.GetOrdinal("propiedad_id")),
                        Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                        Precio = reader.GetInt32(reader.GetOrdinal("precio")),
                        Direccion = reader.GetString(reader.GetOrdinal("direccion"))
                    });
                }
                return new Response2<List<PropiedadModel>>(lista);
            } catch (Exception ex){ return new Response2<List<PropiedadModel>>(ex);
            }
        }


    }
}
