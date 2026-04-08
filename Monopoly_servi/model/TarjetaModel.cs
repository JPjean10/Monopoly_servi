namespace Monopoly_servi.Models
{
    public class TarjetaModel
    {
        public int TarjetaId { get; set; }
        public int JugadorId { get; set; }
        public string? Codigo { get; set; }
        public double Monto { get; set; }
    }
}
