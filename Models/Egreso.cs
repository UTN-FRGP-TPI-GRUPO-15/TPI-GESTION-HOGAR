namespace TPI_GESTION_HOGAR.Models
{
    public class Egreso
    {
        public required int ID { get; set; }
        public required DateOnly Fecha { get; set; }
        public  string? ApellidoRef { get; set; }
        public  string? NombreRef { get; set; }
        public  int? DNIRef { get; set; }
        public string? TelefonoRef { get; set; }
        public string? DomicilioRef { get; set; }
        public string? LocalidadRef { get; set; }


        //FK
        public required int RegistroId { get; set; }
        
        //Relacion
        public  Registro? Registro { get; set; }

    }
}
