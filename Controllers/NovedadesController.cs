using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TPI_GESTION_HOGAR.Datos;
using TPI_GESTION_HOGAR.Models;

namespace TPI_GESTION_HOGAR.Controllers
{
    public class NovedadesController : Controller
    {
        private readonly AppDbContext _context;

        public NovedadesController(AppDbContext context)
        {
            _context = context;
        }

        // PANTALLA PRINCIPAL DEL LIBRO
        [HttpGet]
        public async Task<IActionResult> Index(DateTime? fechaBusqueda)
        {
            // Empezamos la consulta
            var query = _context.Novedades.Include(n => n.Personal).AsQueryable();

            // Lógica del buscador por fecha
            if (fechaBusqueda.HasValue)
            {
                query = query.Where(n => n.FechaHora.Date == fechaBusqueda.Value.Date);
                ViewBag.FechaActual = fechaBusqueda.Value.ToString("yyyy-MM-dd");
            }
            else
            {
                // Por defecto muestra las últimas novedades sin filtro estricto de hoy
                ViewBag.FechaActual = "";
            }

            // Ordenamos desde lo más nuevo a lo más viejo
            var historial = await query.OrderByDescending(n => n.FechaHora).ToListAsync();

            // Mandamos el personal para el formulario de carga
            var personalActivo = await _context.Personal
                .Where(p => p.Activo)
                .Select(p => new { Id = p.Id, Nombre = p.Apellido + ", " + p.Nombre })
                .OrderBy(p => p.Nombre)
                .ToListAsync();

            ViewBag.PersonalActivo = new SelectList(personalActivo, "Id", "Nombre");

            return View(historial);
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Novedad nuevaNovedad)
        {
            nuevaNovedad.FechaHora = DateTime.Now; 

          
            ModelState.Remove("Personal");

            if (ModelState.IsValid)
            {
                _context.Novedades.Add(nuevaNovedad);
                await _context.SaveChangesAsync();

                TempData["MensajeExito"] = "Registro anexado al Libro de Guardia correctamente.";
            }
            else
            {
                TempData["MensajeError"] = "Ocurrió un error al intentar guardar el registro.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}