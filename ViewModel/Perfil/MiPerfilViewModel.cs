namespace TPI_GESTION_HOGAR.ViewModel.Perfil
{
    public class MiPerfilViewModel
    {
        // Datos de Personal
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public int Legajo { get; set; }
        public required string DNI { get; set; }
        public required string Nacionalidad { get; set; }
        public DateOnly FechaNac { get; set; }
        public string? Telefono { get; set; }
        public string? Domicilio { get; set; }
        public string? Localidad { get; set; }

        // Datos de Usuario
        public required string Email { get; set; }
        public required string NombreUsuario { get; set; }
    }
}
