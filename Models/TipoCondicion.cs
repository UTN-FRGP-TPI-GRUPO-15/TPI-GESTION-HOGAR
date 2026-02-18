namespace TPI_GESTION_HOGAR.Models
{
    public class TipoCondicion
    {
        public required int Id { get; set; }
        public required string Descripcion { get; set; }
        public List<Condicion>? Condiciones { get; set; } = null;
    }
}
