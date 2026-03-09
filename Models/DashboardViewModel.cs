namespace TPI_GESTION_HOGAR.Models
{
    public class DashboardViewModel
    {
        public int TotalMujeres { get; set; }
        public int TotalMenores { get; set; }
        public int HabitacionesOcupadas { get; set; }
        public int HabitacionesTotales { get; set; }
        public List<Registro> IngresosActivos { get; set; } = new List<Registro>();      

        public List<Turno> OperadorasTurnoAnterior { get; set; } = new();
        public List<Turno> OperadorasTurnoActual { get; set; } = new();
        public List<Turno> OperadorasTurnoSiguiente { get; set; } = new();
        public List<Novedad> UltimasNovedades { get; set; } = new();
        public List<Recordatorio> RecordatoriosActivos { get; set; } = new();
    }
}
