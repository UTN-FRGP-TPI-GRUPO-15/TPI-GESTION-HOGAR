using System.ComponentModel.DataAnnotations;

namespace TPI_GESTION_HOGAR.Models
{
    public class Novedad
    {
        public int Id { get; set; }

        public DateTime FechaHora { get; set; }

        [Required(ErrorMessage = "Debe escribir la novedad.")]
        public required string Descripcion { get; set; }

        public bool EsPaseDeGuardia { get; set; }

       
        public required int PersonalId { get; set; }
        public Personal? Personal { get; set; }
    }
}