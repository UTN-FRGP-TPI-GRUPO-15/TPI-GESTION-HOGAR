using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TPI_GESTION_HOGAR.Models;
using TPI_GESTION_HOGAR.Datos;

namespace TPI_GESTION_HOGAR.Controllers
{
    public class HabitacionesController : Controller
    {
        private readonly AppDbContext _context;

        public HabitacionesController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Traemos las habitaciones con los ingresos activos, sus mujeres y los menores a cargo
            var habitaciones = await _context.Habitaciones
                .Include(h => h.Registros.Where(r => r.Estado == true))
                    .ThenInclude(r => r.Mujer)
                        .ThenInclude(m => m.Hijos) 
                .OrderBy(h => h.NroHabitacion)
                .ToListAsync();

            return View(habitaciones);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null) return NotFound();

            var habitacion = await _context.Habitaciones.FindAsync(id);
            if (habitacion == null) return NotFound();

            return View(habitacion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Habitacion habModificada)
        {
            if (id != habModificada.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var habOriginal = await _context.Habitaciones.FindAsync(id);
                if (habOriginal == null) return NotFound();

                // PISAMOS SOLO LO QUE PUEDE CAMBIAR (El número de cuarto no se toca)
                habOriginal.Capacidad = habModificada.Capacidad;
                habOriginal.Estado = habModificada.Estado; // true = Operativa, false = Mantenimiento

                await _context.SaveChangesAsync();
                TempData["MensajeExito"] = $"La Habitación {habOriginal.NroHabitacion} fue actualizada.";

                return RedirectToAction(nameof(Index));
            }

            return View(habModificada);
        }
    }
}
