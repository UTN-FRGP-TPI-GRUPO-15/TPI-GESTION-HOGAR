namespace TPI_GESTION_HOGAR.Models
{
    public class TipoTurno
    {
        public required int Id { get; set; }
        public required string Descripcion { get; set; }

        public Turno? Turno { get; set; }   = null;
    }
}
