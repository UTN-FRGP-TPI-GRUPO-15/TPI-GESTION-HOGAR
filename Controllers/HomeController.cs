using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TPI_GESTION_HOGAR.Datos;
using TPI_GESTION_HOGAR.Models;

namespace TPI_GESTION_HOGAR.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var modelo = new DashboardViewModel();

            // 1. Traemos los ingresos activos con toda la info colgada
            modelo.IngresosActivos = await _context.Registros
                .Include(r => r.Mujer)
                    .ThenInclude(m => m.Hijos)
                .Include(r => r.Habitacion)
                .Where(r => r.Estado == true)
                .OrderByDescending(r => r.Fecha)
                .ToListAsync();

            // 2. Calculamos los contadores de personas
            modelo.TotalMujeres = modelo.IngresosActivos.Count;
            modelo.TotalMenores = modelo.IngresosActivos.Sum(r => r.Mujer?.Hijos?.Count ?? 0);

            // 3. Calculamos la ocupación de habitaciones operativas
            var habitacionesActivas = await _context.Habitaciones
                .Include(h => h.Registros.Where(r => r.Estado == true))
                .Where(h => h.Estado == true)
                .ToListAsync();

            modelo.HabitacionesTotales = habitacionesActivas.Count;
            // Contamos como "ocupada" si tiene al menos un registro adentro
            modelo.HabitacionesOcupadas = habitacionesActivas.Count(h => h.Registros.Any());

            return View(modelo);
        }
    }
}