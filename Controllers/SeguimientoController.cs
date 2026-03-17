using Microsoft.AspNetCore.Mvc;
using TPI_GESTION_HOGAR.Datos;
using TPI_GESTION_HOGAR.Models;

namespace TPI_GESTION_HOGAR.Controllers
{
    public class SeguimientosController : Controller
    {
        private readonly AppDbContext _context;

        public SeguimientosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CargaRapida(int registroId, string categoria, string descripcion, int personalId)
        {
            var nuevaNovedad = new Seguimiento
            {
                RegistroId = registroId,
                PersonalId = personalId,
                Categoria = categoria,
                Descripcion = descripcion,
                FechaHora = DateTime.Now 
            };

            _context.Seguimientos.Add(nuevaNovedad);
            await _context.SaveChangesAsync();

            TempData["MensajeExito"] = "Novedad guardada exitosamente en el legajo.";

            
            return RedirectToAction("Index", "Home");
        }
    }
}