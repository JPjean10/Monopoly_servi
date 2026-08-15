namespace Monopoly_servi.model
{
    public class CartaTrampaJugadorModel
    {
        public int CartaJugadorId { get; set; }
        public int JugadorId { get; set; }
        public int CartaId { get; set; }
        public CartaTrampaModel CartaTrampaModel { get; set; }
    }
}
