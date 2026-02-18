namespace TPI_GESTION_HOGAR.Models
{
    public class Rol
    {
        public required int  Id { get; set; }
        public required string Descripcion { get; set; }


        public Usuario? Usuario { get; set; }
    }
}
