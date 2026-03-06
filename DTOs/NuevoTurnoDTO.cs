using System.ComponentModel.DataAnnotations;

namespace TPI_GESTION_HOGAR.DTOs
{
    public class NuevoTurnoDTO
    {
        public DateOnly Fecha { get; set; }
        public int TipoTurnoId { get; set; }
        public int? PersonalId { get; set; }
    }
}
