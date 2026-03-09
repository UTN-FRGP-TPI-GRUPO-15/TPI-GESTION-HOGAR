using System.ComponentModel.DataAnnotations;

namespace TPI_GESTION_HOGAR.DTOs
{
    public class EditarPersonalDTO
    {
        //Datos de Personal
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Legajo inválido")]
        public int Legajo { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public required string Apellido { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public required string Nombre { get; set; }
        [Required]
        [RegularExpression(@"^\d{7,10}$", ErrorMessage = "DNI inválido")]
        public required string DNI { get; set; }
        [Required]
        public required string Nacionalidad { get; set; }
        [Required]
        public DateOnly? FechaNac { get; set; }
        [Phone(ErrorMessage = "Número de teléfono inválido")]
        public string? Telefono { get; set; }
        public string? Domicilio { get; set; }
        public string? Provincia { get; set; }
        public string? Localidad { get; set; }   
    }
}
