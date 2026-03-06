using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace TPI_GESTION_HOGAR.Models
{
    [Index(nameof(Fecha), nameof(TipoTurnoId), IsUnique = true)]
    public class Turno
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Debe ingresar la fecha")]
        public required DateOnly Fecha { get; set; }

        //FK
        [Required(ErrorMessage = "Debe ingresar el horario")]
        public required int TipoTurnoId { get; set; }
        [Required(ErrorMessage = "Debe ingresar el personal")]
        public required int  PersonalId { get; set; }

        //Relaciones
        public TipoTurno? TipoTurno { get; set; }
        public Personal? Personal { get; set; }
    }
}
