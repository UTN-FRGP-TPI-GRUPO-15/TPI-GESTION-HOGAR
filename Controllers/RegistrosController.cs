using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TPI_GESTION_HOGAR.Datos;
using TPI_GESTION_HOGAR.Models;
using System.Threading.Tasks;


namespace TPI_GESTION_HOGAR.Controllers
{
    public class RegistrosController : Controller
    {
        private readonly AppDbContext _context;

        public RegistrosController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Detalles(int? id)
        {
            if (id == null) return NotFound();

            var registro = await _context.Registros
                .Include(r => r.Mujer)
                    .ThenInclude(m => m.Hijos)     
                .Include(r => r.Habitacion)        
                .Include(r => r.Agresores)         
                .Include(r => r.Denuncias)
                .ThenInclude(d => d.Medida)
                .ThenInclude(m => m.TipoMedida)

                .FirstOrDefaultAsync(r => r.Id == id);

            if (registro == null) return NotFound();

            return View(registro);
        }
        [HttpGet]
        public async Task<IActionResult> Legajo(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }
            var registro = await _context.Registros.Include(r => r.Mujer).Include(r => r.Seguimientos).ThenInclude(s => s.Personal).FirstOrDefaultAsync(m => m.Id == id);

            return View(registro);
        }

        [HttpGet]
        public async Task<IActionResult> AsignarHabitacion(int? id)
        {
            if (id == null) return NotFound();

     
            var registro = await _context.Registros
                .Include(r => r.Mujer)
                    .ThenInclude(m => m.Hijos)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (registro == null) return NotFound();

            
            int camasNecesarias = 1 + (registro.Mujer?.Hijos?.Count ?? 0);
            var habitacionesActivas = await _context.Habitaciones
                .Include(h => h.Registros.Where(r => r.Mujer != null && r.Mujer.estado == true))
                .Where(h => h.Estado == true)
                .ToListAsync();

            var listaHabitaciones = habitacionesActivas
                .Where(h =>
                 
                    h.Id == registro.HabitacionId ||
                   
                    (!h.Registros.Any() && h.Capacidad >= camasNecesarias)
                )
                .Select(h => new SelectListItem
                {
                    Value = h.Id.ToString(),
                    Text = $"Habitación {h.NroHabitacion} - (Capacidad total: {h.Capacidad} plazas)",
                    Selected = h.Id == registro.HabitacionId
                }).ToList();

            ViewBag.Habitaciones = listaHabitaciones;
            ViewBag.CamasNecesarias = camasNecesarias;

            return View(registro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AsignarHabitacion(int id, int? habitacionId)
        {
            var registro = await _context.Registros.FindAsync(id);
            if (registro == null || registro.Id != id) return NotFound();

            // Actualizamos la habitación (Si elige "Sin asignar", habitacionId viaja como null y es válido gracias a tu migración)
            registro.HabitacionId = habitacionId;

            await _context.SaveChangesAsync();

            TempData["MensajeExito"] = "La asignación de cama se actualizó correctamente.";

            // Al terminar, la devolvemos al Dashboard principal
            return RedirectToAction("Index", "Home");
        }

        
    }
}
