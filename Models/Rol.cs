namespace TPI_GESTION_HOGAR.Models
{
    public class Rol
    {
        public int  Id { get; set; }
        public required string Descripcion { get; set; }


        public List<Usuario>? Usuarios { get; set; }
    }
}
