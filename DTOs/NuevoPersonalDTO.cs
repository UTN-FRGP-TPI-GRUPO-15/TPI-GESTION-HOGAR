using System.ComponentModel.DataAnnotations;

namespace TPI_GESTION_HOGAR.DTOs
{
    public class NuevoPersonalDTO
    {
        //Datos de Personal
        [Required]
        public int Legajo { get; set; }
        [Required]
        public required string Apellido { get; set; }
        [Required]
        public required string Nombre { get; set; }
        [Required]
        public required string DNI { get; set; }
        [Required]
        public required string Nacionalidad { get; set; }
        [Required]
        public DateOnly? FechaNac { get; set; }
        public string? Telefono { get; set; }
        public string? Domicilio { get; set; }
        public string? Localidad { get; set; }

        //Datos de Usuario
        [Required]
        public required string NombreUsuario { get; set; }
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string ClavePlana { get; set; }
        [Required]
        public int RolId { get; set; }
    }
}
