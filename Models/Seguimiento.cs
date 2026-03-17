using System.ComponentModel.DataAnnotations;

namespace TPI_GESTION_HOGAR.Models
{
    public class Seguimiento
    {
        public int Id { get; set; }

        public DateTime FechaHora { get; set; } = DateTime.Now; 

        [Required(ErrorMessage = "Debe escribir el detalle de la observación.")]
        public required string Descripcion { get; set; }

      
        public string? Categoria { get; set; }

        
        public int RegistroId { get; set; }
        public Registro? Registro { get; set; }

       
        public int PersonalId { get; set; }
        public Personal? Personal { get; set; }
    }
}