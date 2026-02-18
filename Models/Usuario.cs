namespace TPI_GESTION_HOGAR.Models
{
    public class Usuario
    {
        public required int Id { get; set; }
        public required string NombreUsuario { get; set; }
        public required string Clave { get; set; }

        //FK
        public required int PersonalId { get; set; }
        public required int RolId { get; set; }

        //Relaciones
        public required Personal Personal { get; set; }
        public required Rol Rol { get; set; }
    }
}
