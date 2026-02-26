using System.ComponentModel.DataAnnotations;

namespace TPI_GESTION_HOGAR.Models
{
    public class Condicion
    {
        public int  Id { get; set; }

        //FK
        [Required]
        public int MujerId { get; set; }
        [Required]
        public int TipoCondicionId { get; set; }
        [Required]
        public int ObservacionCondicionId { get; set; }
        //Relaciones
        public  Mujer? Mujer { get; set; }
        public  TipoCondicion? TipoCondicion { get; set; }
        public  ObservacionCondicion? ObservacionCondicion { get; set; }


    }
}
