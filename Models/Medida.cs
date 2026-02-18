namespace TPI_GESTION_HOGAR.Models
{
    public class Medida
    {
        public required int  Id { get; set; }
        public  required string Descripcion { get; set; }

        //FK
        public required int DenunciaId  { get; set; }
        //RELACION
        public Denuncia? Denuncia { get; set; }
    }
}
