using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TPI_GESTION_HOGAR.Datos; // O donde esté tu AppDbContext
using TPI_GESTION_HOGAR.Models;

namespace TPI_GESTION_HOGAR.Servicios
{
    public class PersonalService
    {
        private readonly AppDbContext _context;

        public PersonalService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SelectList> ObtenerPersonalAutorizadoAsync()
        {
            TimeZoneInfo zonaArgentina;
            try
            {
                zonaArgentina = TimeZoneInfo.FindSystemTimeZoneById("Argentina Standard Time");
            }
            catch (TimeZoneNotFoundException)
            {
                zonaArgentina = TimeZoneInfo.FindSystemTimeZoneById("America/Argentina/Buenos_Aires"); 
            }

            DateTime ahoraEnArgentina = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zonaArgentina);

           
            var hoy = DateOnly.FromDateTime(ahoraEnArgentina);
            var horaActual = ahoraEnArgentina.Hour;

            string turnoDefinido = "";

            if (horaActual >= 8 && horaActual < 14)
            {
                turnoDefinido = "mañana";
            }
            else if (horaActual >= 14 && horaActual < 20)
            {
                turnoDefinido = "tarde";
            }
            else
            {
                turnoDefinido = "noche";

                if (horaActual < 8)
                {
                    hoy = hoy.AddDays(-1);
                }
            }

            var turnoActual = await _context.Turnos
         .Include(t => t.TipoTurno)
         .Include(t => t.Personal)
         .Include(t => t.PersonalOpc)
         .Where(t => t.Fecha == hoy && t.TipoTurno.Descripcion.ToLower().Contains(turnoDefinido))
         .FirstOrDefaultAsync();

            var personalPermitido = new List<Personal>();

            
            if (turnoActual != null)
            {
                if (turnoActual.Personal != null) personalPermitido.Add(turnoActual.Personal);
                if (turnoActual.PersonalOpc != null) personalPermitido.Add(turnoActual.PersonalOpc);
            }

            var equipoTecnico = await _context.Personal
                .Include(p => p.Usuario)       
                    .ThenInclude(u => u.Rol)    
                .Where(p => p.Activo == true && p.Usuario.Rol.Descripcion == "Equipo Tecnico")
                .ToListAsync();
            personalPermitido.AddRange(equipoTecnico);

            var listaFinal = personalPermitido
                .GroupBy(p => p.Id)
                .Select(g => g.First())
                .Select(p => new
                {
                    Id = p.Id,
                    
                    Nombre = $"{ p.Nombre} {p.Apellido}"
                })
                .OrderBy(p => p.Nombre)
                .ToList();

            return new SelectList(listaFinal, "Id", "Nombre");
        }
    }
}