using System;
using System.Collections.Generic;
using TPI_GESTION_HOGAR.Models;

namespace TPI_GESTION_HOGAR.ViewModels
{
    public class ReporteViewModel
    {

        public string? Provincia { get; set; }
        public string? Localidad { get; set; }
        public int? CantidadHijosMinima { get; set; }
        public bool SoloActivas { get; set; }
        public bool SoloDeLaCosta { get; set; }


        public DateOnly? FechaDesde { get; set; }
        public DateOnly? FechaHasta { get; set; }

        
        public List<Registro> Resultados { get; set; } = new List<Registro>();
        public int TotalMujeres { get; set; }
        public int TotalMenores { get; set; }
    }
}