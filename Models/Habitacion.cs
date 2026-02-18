namespace TPI_GESTION_HOGAR.Models
{
    public class Habitacion
    {
        public required int Id { get; set; }
        public required int NroHabitacion { get; set; }
        public int? Capacidad { get; set; }
        public bool Estado { get; set; }
       
        //RELACION
        public List<Registro>? Registros { get; set; }
    }
}
