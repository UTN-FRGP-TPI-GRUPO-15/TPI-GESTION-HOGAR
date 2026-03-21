namespace TPI_GESTION_HOGAR.ViewModels
{
    public class EstadisticasViewModel
    {
        public int TotalRegistros { get; set; }

        
        public double PromedioEdadMujeres { get; set; }
        public double PromedioEdadHijos { get; set; }
        public double PromedioDiasEstadia { get; set; }

        
        public int MujeresConHijos { get; set; }
        public int MujeresSinHijos { get; set; }

        public int ConConsumo { get; set; }
        public int SinConsumo { get; set; }

        public int DeLaCosta { get; set; }
        public int DeAfuera { get; set; }
    }
}