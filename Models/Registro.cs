using System.ComponentModel.DataAnnotations;

namespace TPI_GESTION_HOGAR.Models
{
    public class Registro
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo Fecha es obligatorio.")]
        public DateOnly Fecha { get; set; }
        public bool Estado { get; set; }

        //FK
        public int MujerID { get; set; }
        public int? HabitacionId { get; set; }

        //Relaciones principales
        public Mujer? Mujer { get; set; }
        public Habitacion? Habitacion { get; set; }
        //Relacion 1 a 1 
        public Egreso? Egreso { get; set; }
        //Relacion 1 a muchos
        public List<Denuncia>? Denuncias { get; set; }
        public List<Observacion>? Observaciones { get; set; }
        public List<Agresor>? Agresores { get; set; }
        public ICollection<Seguimiento>? Seguimientos { get; set; }
        public ICollection<Documento> Documentos { get; set; }










    }
}
