using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TPI_GESTION_HOGAR.Models
{
    public class Hijo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public  int ID { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [Range(1000000, 99999999, ErrorMessage = "Ingrese un DNI válido (entre 7 y 8 números, sin puntos).")]
        public  int DNI { get; set; }
        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public  string Apellido { get; set; }
        [Required(ErrorMessage = "El Nombte es obligatorio.")]
        public  string Nombre { get; set; }
        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        public  DateOnly FechaNac { get; set; }

        //FK
      
        public required int MujerId { get; set; }

        //RELACION
        
        public Mujer? Mujer { get; set; }

        public int Edad
        {
            get
            {
                var hoy = DateOnly.FromDateTime(DateTime.Today);
                int edadCalculada = hoy.Year - FechaNac.Year;


                if (FechaNac > hoy.AddYears(-edadCalculada))
                {
                    edadCalculada--;
                }

                return edadCalculada;
            }
        }
    }
}
