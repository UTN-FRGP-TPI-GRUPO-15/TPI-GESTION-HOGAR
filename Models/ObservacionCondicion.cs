namespace TPI_GESTION_HOGAR.Models
{
    public class ObservacionCondicion
    {
        public required int Id { get; set; }
        public required string Descripcion { get; set; }
        public List<Condicion>? Condicion { get; set; } 
    }
}
