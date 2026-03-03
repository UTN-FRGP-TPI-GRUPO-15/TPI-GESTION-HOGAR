using System.ComponentModel.DataAnnotations;

namespace TPI_GESTION_HOGAR.Models
{
    public class ObservacionCondicion
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Descripcion { get; set; }
        public List<Condicion>? Condicion { get; set; } 
    }
}
