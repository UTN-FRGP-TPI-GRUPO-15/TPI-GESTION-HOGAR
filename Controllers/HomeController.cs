using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TPI_GESTION_HOGAR.Datos;
using TPI_GESTION_HOGAR.Models;
using TPI_GESTION_HOGAR.Servicios;

namespace TPI_GESTION_HOGAR.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly PersonalService _personalService;

        public HomeController(AppDbContext context,PersonalService personalService)
        {
            _context = context;
            _personalService = personalService;

        }   

        public async Task<IActionResult> Index()
        {
            var modelo = new DashboardViewModel();

            // 1. Traemos los ingresos activos con toda la info colgada
            modelo.IngresosActivos = await _context.Registros
                .Include(r => r.Mujer)
                    .ThenInclude(m => m.Hijos)
                .Include(r => r.Habitacion)
                .Where(r => r.Mujer != null && r.Mujer.Estado == true)
                .Where(r => !_context.Egresos.Any(e => e.RegistroId == r.Id))
                .OrderByDescending(r => r.Fecha)
                .ToListAsync();

            // 2. Calculamos los contadores de personas
            modelo.TotalMujeres = modelo.IngresosActivos.Count;
            modelo.TotalMenores = modelo.IngresosActivos.Sum(r => r.Mujer?.Hijos?.Count ?? 0);

            var habitacionesActivas = await _context.Habitaciones
            .Include(h => h.Registros.Where(r => r.Mujer != null && r.Mujer.Estado == true))
            .Where(h => h.Estado == true)
            .ToListAsync();
            modelo.HabitacionesTotales = habitacionesActivas.Count;          
            modelo.HabitacionesOcupadas = habitacionesActivas.Count(h => h.Registros.Any());

            
            int horaActual = DateTime.Now.Hour;
            DateOnly fechaHoy = DateOnly.FromDateTime(DateTime.Now);
            DateOnly fechaAyer = fechaHoy.AddDays(-1);
            DateOnly fechaManana = fechaHoy.AddDays(1);

            string descActual, descAnterior, descSiguiente;
            DateOnly fechaTurnoActual = fechaHoy;
            DateOnly fechaTurnoAnterior = fechaHoy;
            DateOnly fechaTurnoSiguiente = fechaHoy;

            // 2. Definimos los bloques horarios reales (8 a 14, 14 a 20, 20 a 8)
            if (horaActual >= 8 && horaActual < 14)
            {
                // TURNO MAÑANA (08:00 a 13:59)
                descActual = "Mañana";
                descAnterior = "Noche";
                fechaTurnoAnterior = fechaAyer; // La noche arrancó ayer a las 20hs
                descSiguiente = "Tarde";
            }
            else if (horaActual >= 14 && horaActual < 20)
            {
                // TURNO TARDE (14:00 a 19:59)
                descActual = "Tarde";
                descAnterior = "Mañana";
                descSiguiente = "Noche";
            }
            else
            {
                // TURNO NOCHE (20:00 a 23:59 o 00:00 a 07:59)
                descActual = "Noche";
                descAnterior = "Tarde";
                descSiguiente = "Mañana";

                if (horaActual < 8)
                {
                    // Si estamos en la madrugada (ej: 03:00 AM)
                    // El turno actual pertenece a la fecha de ayer
                    fechaTurnoActual = fechaAyer;
                    fechaTurnoAnterior = fechaAyer; // La tarde de ayer
                    fechaTurnoSiguiente = fechaHoy; // La mañana de hoy
                }
                else
                {
                    // Si son entre las 20:00 y las 23:59
                    // El turno actual pertenece a la fecha de hoy
                    fechaTurnoAnterior = fechaHoy;
                    fechaTurnoSiguiente = fechaManana; // La mañana de mañana
                }
            }

            // 3. Traemos los turnos de la base (Ayer, Hoy y Mañana)
            // Al incluir el Personal, ya traemos a las operadoras
            var turnosRecientes = await _context.Turnos
                .Include(t => t.Personal)
                .Include(t => t.TipoTurno)
                .Include(t => t.PersonalOpc)
                .Where(t => t.Fecha >= fechaAyer && t.Fecha <= fechaManana)
                .ToListAsync();

            // 4. Repartimos en las 3 listas 
            modelo.OperadorasTurnoActual = turnosRecientes
                .Where(t => t.Fecha == fechaTurnoActual && t.TipoTurno.Descripcion.Contains(descActual, StringComparison.OrdinalIgnoreCase))
                .ToList();

            modelo.OperadorasTurnoAnterior = turnosRecientes
                .Where(t => t.Fecha == fechaTurnoAnterior && t.TipoTurno.Descripcion.Contains(descAnterior, StringComparison.OrdinalIgnoreCase))
                .ToList();

            modelo.OperadorasTurnoSiguiente = turnosRecientes
                .Where(t => t.Fecha == fechaTurnoSiguiente && t.TipoTurno.Descripcion.Contains(descSiguiente, StringComparison.OrdinalIgnoreCase))
                .ToList();

            // 5. Traemos los Recordatorios Activos (Los que Resuelto == false)
            modelo.RecordatoriosActivos = await _context.Recordatorios
                .Include(r => r.Personal)
                .Where(r => !r.Resuelto)
                .OrderBy(r => r.FechaLimite)
                .ToListAsync();

            // La lista de Personal Activo
            ViewBag.PersonalActivo = await _personalService.ObtenerPersonalAutorizadoAsync();


            var residentesActivas = await _context.Registros
                .Include(r => r.Mujer)
                .Where(r => r.Mujer != null && r.Mujer.Estado == true)
                .Select(r => new { Id = r.Id, Nombre = r.Mujer.Apellido + ", " + r.Mujer.Nombre })
                .OrderBy(r => r.Nombre)
                .ToListAsync();

            ViewBag.ResidentesActivas = new SelectList(residentesActivas, "Id", "Nombre");
            return View(modelo);
        }
    }
}   