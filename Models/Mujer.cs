using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace TPI_GESTION_HOGAR.Models
{
    public class Mujer
    {
        
        public int ID { get; set; }
        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [RegularExpression(@"^\d{7,8}$", ErrorMessage = "Ingrese un DNI válido.")]
        public  int DNI { get; set; }
        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public  string Apellido { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public  string Nombre { get; set; }
        [Required(ErrorMessage = "La nacionalidad es obligatoria.")]
        public  string Nacionalidad { get; set; }
        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        public  DateOnly FechaNac { get; set; }
        public string? Genero { get; set; }
        public string? NivelEducativo { get; set; }
        public string? Ocupacion { get; set; }
        public string? Telefono { get; set; }
        public string? Domicilio { get; set; }
        public string? Provincia { get; set; }
        public string? Localidad { get; set; }
        public bool Estado { get; set; }

        public ICollection<Hijo> Hijos { get; set; } = new List<Hijo>();
        public ICollection<Condicion> Condiciones { get; set; } = new List<Condicion>();

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
