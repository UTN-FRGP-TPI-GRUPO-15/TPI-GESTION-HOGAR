using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TPI_GESTION_HOGAR.Models
{
    public class Documento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string NombreArchivo { get; set; }

        [Required]
        public string RutaArchivo { get; set; }

        
        [StringLength(100)]
        public string TipoDocumento { get; set; } 

        public DateTime FechaSubida { get; set; }

        [Required]
        public int RegistroId { get; set; }

        [ForeignKey("RegistroId")]
        public Registro Registro { get; set; }
    }
}