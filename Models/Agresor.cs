using System.ComponentModel.DataAnnotations;

namespace TPI_GESTION_HOGAR.Models
{
    public class Agresor
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [Range(1000000, 99999999, ErrorMessage = "Ingrese un DNI válido (entre 7 y 8 números, sin puntos).")]
        public int DNI { get; set; }
        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public string Apellido { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "La nacionalidad es obligatoria.")]
        public string Nacionalidad { get; set; }
        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        public DateOnly FechaNac { get; set; }
        public string? Genero { get; set; }
        public string? NivelEducativo { get; set; }
        public string? Ocupacion { get; set; }
        public string? Telefono { get; set; }
        public string? Domicilio { get; set; }
        public string? Localidad { get; set; }

        //FK
        public required int RegistroId { get; set; }
        //RELACION
        public  Registro? Registro { get; set; }
    }
}
