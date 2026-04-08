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

                // _ = NotificarWebSocket(_env["Urls:WebSocketInser"]);

                string mensaje = resultado?.ToString() ?? "Jugador insertado correctamente.";

                return new Response2<bool>(201, mensaje, true);
            }
            catch (SqlException ex)
            {
                // En C#, SqlState se maneja por Number o State
                return new Response2<bool>(ex, ex.Number);
            }
        }

    /*    public Task<Response2<bool>> InsertarJugador(JugadorModel jugador)
        {
            throw new NotImplementedException();
        }

        private async Task NotificarWebSocket(string? url)
        {
            if (string.IsNullOrEmpty(url)) return;
            try {
                using var client = new HttpClient();
                await client.PostAsync(url, new StringContent(""));
            } catch (Exception e) { Console.WriteLine("Error notificando WebSocket: " + e.Message); }
        }*/
    }
}
