namespace TPI_GESTION_HOGAR.Models
{
    public class Personal
    {
        public int ID { get; set; }
        public required int Legajo { get; set; }
        public required string Apellido { get; set; }
        public required string Nombre { get; set; }
        public required int DNI { get; set; }
        public required string Nacionalidad { get; set; }
        public required DateOnly FechaNac { get; set; }
        public string? Telefono { get; set; }
        public string? Domicilio { get; set; }
        public string? Localidad { get; set; }
        public bool estado { get; set; }

        public Usuario? Usuario { get; set; }

        public List<Turno>? Turno { get; set; }
    }
}
