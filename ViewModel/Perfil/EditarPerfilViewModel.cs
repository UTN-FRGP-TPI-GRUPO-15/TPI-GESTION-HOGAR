using System.ComponentModel.DataAnnotations;

namespace TPI_GESTION_HOGAR.ViewModel.Perfil
{
    public class EditarPerfilViewModel
    {
        [Required(ErrorMessage = "Debe ingresar un apellido válido")]
        [StringLength(50, MinimumLength = 2)]
        public required string Apellido { get; set; }
        [Required(ErrorMessage = "Debe ingresar un nombre válido")]
        [StringLength(50, MinimumLength = 2)]
        public required string Nombre { get; set; }
        [Required(ErrorMessage = "Debe ingresar una nacionalidad válida")]
        public required string Nacionalidad { get; set; }
        [Required]
        public DateOnly FechaNac { get; set; }
        [Phone(ErrorMessage = "Número de teléfono inválido")]
        public string? Telefono { get; set; }
        public string? Domicilio { get; set; }
        public string? Localidad { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public required string Email { get; set; }
    }
}
