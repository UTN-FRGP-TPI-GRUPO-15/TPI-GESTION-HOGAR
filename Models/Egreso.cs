using System.ComponentModel.DataAnnotations;

namespace TPI_GESTION_HOGAR.Models
{
    public class Egreso
    {
        public  int ID { get; set; }
        [Required]
        public  DateOnly Fecha { get; set; }
        public  string? ApellidoRef { get; set; }
        public  string? NombreRef { get; set; }
        public  int? DNIRef { get; set; }
        public string? TelefonoRef { get; set; }
        public string? DomicilioRef { get; set; }
        public string? LocalidadRef { get; set; }


        //FK
        public int RegistroId { get; set; }
        
        //Relacion
        public  Registro? Registro { get; set; }

    }
}
