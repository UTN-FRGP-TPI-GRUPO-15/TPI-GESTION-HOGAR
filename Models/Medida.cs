using System.ComponentModel.DataAnnotations;

namespace TPI_GESTION_HOGAR.Models
{
    public class Medida
    {
        public  int  Id { get; set; }
        [Required]
        public  int TipoMedidaId { get; set; }
        public TipoMedida? TipoMedida { get; set; }

        //FK
        public required int DenunciaId  { get; set; }
        //RELACION
        public Denuncia? Denuncia { get; set; }
    }
}
