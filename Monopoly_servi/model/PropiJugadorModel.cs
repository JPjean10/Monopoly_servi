namespace Monopoly_servi.model
{
    public class PropiJugadorModel
    {
        public int PropiedadJugadorId { get; set; }
        public int JugadorId { get; set; }
        public int PropiedadId { get; set; }
        public string? Nombre { get; set; }
        public int NivelActual { get; set; }
        public int Renta { get; set; }
        public PropiedadModel? Propiedad { get; set; }
    }

    public class VentaMasivaRequest
    {
        public int JugadorId { get; set; }
        public string PropiedadesIds { get; set; } = string.Empty; // Cadena tipo "3,5,8"
    }
}
