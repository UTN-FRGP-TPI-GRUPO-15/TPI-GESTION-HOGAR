namespace TPI_GESTION_HOGAR.Models
{
    public class Mujer
    {
        public required int ID { get; set; }
        public required int DNI { get; set; }
        public required string Apellido { get; set; }
        public required string Nombre { get; set; }
        public required string Nacionalidad { get; set; }
        public required DateOnly FechaNac { get; set; }
        public string? Genero { get; set; }
        public string? NivelEducativo { get; set; }
        public string? Ocupacion { get; set; }
        public string? Telefono { get; set; }
        public string? Domicilio { get; set; }
        public string? Localidad { get; set; }
        public bool estado { get; set; }

        public List<Hijo>? Hijos { get; set; }
        public List<Condicion>? Condiciones { get; set; }
    }
}
