using System.ComponentModel.DataAnnotations;

namespace Monopoly_servi.Models;

public class JugadorModel
{
    public int JugadorId { get; set; }
    public string Nombre { get; set; }
    public bool EsBanco { get; set; }
    public TarjetaModel? Tarjeta { get; set; }
}