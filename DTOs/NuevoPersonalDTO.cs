using System.ComponentModel.DataAnnotations;

namespace TPI_GESTION_HOGAR.DTOs
{
    public class NuevoPersonalDTO
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
        public string? Localidad { get; set; }

        //Datos de Usuario
        [Required]
        [StringLength(20, MinimumLength = 4)]
        public required string NombreUsuario { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public required string ClavePlana { get; set; }
        [Required]
        [Compare("ClavePlana", ErrorMessage = "Las contraseñas no coinciden")]
        public required string RepetirClavePlana { get; set; }
        [Required]
        public int RolId { get; set; }
    }
}
