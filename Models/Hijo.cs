namespace TPI_GESTION_HOGAR.Models
{
    public class Hijo
    {
        public required int ID { get; set; }
        public required int DNI { get; set; }
        public required string Apellido { get; set; }
        public required string Nombre { get; set; }
        public required DateOnly FechaNac { get; set; }

        //FK
        public required int MujerId { get; set; }

        //RELACION
        public   Mujer? Mujer { get; set; }
    }
}
