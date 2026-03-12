using System.ComponentModel.DataAnnotations;

namespace TPI_GESTION_HOGAR.Models
{
    public class Recordatorio
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe escribir el recordatorio.")]
        public required string Descripcion { get; set; }

        public DateTime FechaLimite { get; set; }

        public bool Importante { get; set; }

        public bool Resuelto { get; set; } 

       
        public required int PersonalId { get; set; }
        public Personal? Personal { get; set; }
        public int? RegistroId { get; set; }
        public Registro? Registro { get; set; }

        
        public string? ResultadoObservacion { get; set; }
    }
}