namespace TPI_GESTION_HOGAR.Models
{
    public class Personal
    {
        public int Id { get; set; }
        public int Legajo { get; set; }
        public required string Apellido { get; set; }
        public required string Nombre { get; set; }
        public required string DNI { get; set; }
        public required string Nacionalidad { get; set; }
        public DateOnly FechaNac { get; set; }
        public string? Telefono { get; set; }
        public string? Domicilio { get; set; }
        public string? Localidad { get; set; }
        public bool Activo { get; set; }

        public Usuario Usuario { get; set; } = null!;

        public ICollection<Turno> Turnos { get; set; } = [];
    }
}
