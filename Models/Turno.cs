namespace TPI_GESTION_HOGAR.Models
{
    public class Turno
    {
        public required int ID { get; set; }
        public required DateOnly Fecha { get; set; }     
        
        //FK
        public required int TipoTurnoId { get; set; }
        public required int  PersonalId { get; set; }

        //Relaciones
        public TipoTurno? TipoTurno { get; set; }
        public Personal? Personal { get; set; }
    }
}
