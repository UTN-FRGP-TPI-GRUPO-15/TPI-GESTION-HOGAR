namespace TPI_GESTION_HOGAR.Models
{
    public class DashboardViewModel
    {
        public int TotalMujeres { get; set; }
        public int TotalMenores { get; set; }
        public int HabitacionesOcupadas { get; set; }
        public int HabitacionesTotales { get; set; }
        public List<Registro> IngresosActivos { get; set; } = new List<Registro>();
    }
}