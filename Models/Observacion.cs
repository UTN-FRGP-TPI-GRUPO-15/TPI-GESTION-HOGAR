namespace TPI_GESTION_HOGAR.Models
{
    public class Observacion
    {
        public required int Id { get; set; }
        public required DateOnly Fecha { get; set; }
        public required string Suceso { get; set; }

        //FK
        public required int RegistroId { get; set; }
        //RELACIOM
        public Registro? Registro { get; set; }
    }   
}
