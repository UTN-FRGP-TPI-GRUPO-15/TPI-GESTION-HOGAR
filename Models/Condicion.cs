namespace TPI_GESTION_HOGAR.Models
{
    public class Condicion
    {
        public required int  Id { get; set; }

        //FK
        public  required int MujerId { get; set; }
        public  required int TipoCondicionId { get; set; }
        public  required int ObservacionCondicionId { get; set; }
        //Relaciones
        public required Mujer Mujer { get; set; }
        public required TipoCondicion TipoCondicion { get; set; }
        public required ObservacionCondicion ObservacionCondicion { get; set; }


    }
}
