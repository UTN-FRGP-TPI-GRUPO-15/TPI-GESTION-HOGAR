using System.ComponentModel.DataAnnotations;

namespace TPI_GESTION_HOGAR.DTOs
{
    public class EditarPerfilDTO
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public required string Apellido { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public required string Nombre { get; set; }
        [Required]
        public required string Nacionalidad { get; set; }
        [Required]
        public DateOnly FechaNac { get; set; }
        [Phone(ErrorMessage = "Número de teléfono inválido")]
        public string? Telefono { get; set; }
        public string? Domicilio { get; set; }
        public string? Localidad { get; set; }
        [EmailAddress(ErrorMessage = "Email inválido")]
        public required string Email { get; set; }
    }
}
