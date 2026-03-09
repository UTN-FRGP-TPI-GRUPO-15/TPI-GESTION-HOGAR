using Microsoft.AspNetCore.Mvc;
using TPI_GESTION_HOGAR.Datos;
using TPI_GESTION_HOGAR.Models;

namespace TPI_GESTION_HOGAR.Controllers
{
    public class RecordatoriosController : Controller
    {
        private readonly AppDbContext _context;

        public RecordatoriosController(AppDbContext context)
        {
            _context = context;
        }

        // GUARDAR NUEVO RECORDATORIO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearDesdeDashboard(Recordatorio nuevoRecordatorio)
        {
            // Nos aseguramos que nazca como "No resuelto"
            nuevoRecordatorio.Resuelto = false;

            // Limpiamos el objeto de navegación para que el ModelState sea válido
            ModelState.Remove("Personal");

            if (ModelState.IsValid)
            {
                _context.Recordatorios.Add(nuevoRecordatorio);
                await _context.SaveChangesAsync();
            }

            // Volvemos al Dashboard sin importar qué pase
            return RedirectToAction("Index", "Home");
        }

       
        [HttpPost]
        public async Task<IActionResult> MarcarResuelto(int id)
        {
            var recordatorio = await _context.Recordatorios.FindAsync(id);
            if (recordatorio != null)
            {
                recordatorio.Resuelto = true;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Home");
        }
    }
}