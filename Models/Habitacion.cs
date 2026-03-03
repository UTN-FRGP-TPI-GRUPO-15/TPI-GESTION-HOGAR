using System.ComponentModel.DataAnnotations;

namespace TPI_GESTION_HOGAR.Models
{
    public class Habitacion
    {
        public  int Id { get; set; }
        [Required]
        public int NroHabitacion { get; set; }
        public int? Capacidad { get; set; }
        public bool Estado { get; set; }
       
        //RELACION
        public List<Registro>? Registros { get; set; }
    }
}
