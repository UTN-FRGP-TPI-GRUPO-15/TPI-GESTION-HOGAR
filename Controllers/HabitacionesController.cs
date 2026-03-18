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
        public async Task<IActionResult> Crear()
        {
            
            int proximoNumero = 1; 

           
            if (await _context.Habitaciones.AnyAsync())
            {
                proximoNumero = await _context.Habitaciones.MaxAsync(h => h.NroHabitacion) + 1;
            }

           
            var nuevaHabitacion = new Habitacion
            {
                NroHabitacion = proximoNumero,
                Estado = true
            };

            return View(nuevaHabitacion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Habitacion habitacion)
        {
            if (ModelState.IsValid)
            {
                _context.Habitaciones.Add(habitacion);
                await _context.SaveChangesAsync();

               
                return RedirectToAction("Index", "Home");
            }

            return View(habitacion);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
           
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
