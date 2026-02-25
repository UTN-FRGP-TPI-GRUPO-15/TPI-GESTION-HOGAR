namespace TPI_GESTION_HOGAR.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public required string NombreUsuario { get; set; }
        public required string Email { get; set; }
        public required string Clave { get; set; }

        //FK
        public int PersonalId { get; set; }
        public int RolId { get; set; }

        //Relaciones
        public Personal Personal { get; set; } = null!;
        public Rol Rol { get; set; } = null!;
    }
}
