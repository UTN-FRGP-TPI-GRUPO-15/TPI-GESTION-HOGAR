namespace TPI_GESTION_HOGAR.Models
{
    public class Denuncia
    {
        public required int ID { get; set; }
        public required DateOnly Fecha { get; set; }
        public int? NroIPP { get; set; }
        public int? NroExp { get; set; }

        //FK
        public required int RegistroId { get; set; }
        //RElacion
        public required Registro Registro { get; set; }
        public required List<Medida> Medida { get; set; }
    }
}
