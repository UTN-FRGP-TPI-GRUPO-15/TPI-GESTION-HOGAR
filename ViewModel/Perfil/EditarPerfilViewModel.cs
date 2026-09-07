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

        [DataType(DataType.Password)]
        [Display(Name = "Contraseña Actual")]
        public string? PasswordActual { get; set; }

        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La nueva contraseña debe tener al menos 6 caracteres.")]
        [Display(Name = "Nueva Contraseña")]
        public string? PasswordNueva { get; set; }

        [DataType(DataType.Password)]
        [Compare("PasswordNueva", ErrorMessage = "La nueva contraseña y la confirmación no coinciden.")]
        [Display(Name = "Confirmar Nueva Contraseña")]
        public string? ConfirmarPasswordNueva { get; set; }
    }
}
