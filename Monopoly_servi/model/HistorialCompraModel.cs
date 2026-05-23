namespace Monopoly_servi.model
{
    public class HistorialCompraModel
    {
        public int historialId { get; set; }
        public int JugadorId { get; set; }
        public int PropiedadId { get; set; }
        public string TipoCompra { get; set; }
        public string Estado { get; set; }
        public string? mensage { get; set; }
        public string? FechaHoraCreacion { get; set; }

        public string? NombreJugador { get; set; }
        public string? NombrePropiedad { get; set; }
    }
}
