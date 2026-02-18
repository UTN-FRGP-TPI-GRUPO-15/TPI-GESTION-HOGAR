namespace TPI_GESTION_HOGAR.Models
{
    public class Registro
    {
        public required int Id { get; set; }
        public required DateOnly Fecha { get; set; }
        public DateOnly? FechaEgreso { get; set; }
        public bool Estado { get; set; }

        //FK
        public required int MujerID { get; set; }
        public required int HabitacionId { get; set; }

        //Relaciones principales
        public Mujer? Mujer { get; set; }
        public Habitacion? Habitacion { get; set; }
        //Relacion 1 a 1 
        public Egreso? Egreso { get; set; }
        //Relacion 1 a muchos
        public List<Denuncia>? Denuncias { get; set; }
        public List<Observacion>? Observaciones { get; set; }
        public List<Agresor>? Agresores { get; set; }
       

        

           


        
        


    }
}
