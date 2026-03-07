using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TPI_GESTION_HOGAR.Datos;
using TPI_GESTION_HOGAR.Models;

namespace TPI_GESTION_HOGAR.Controllers
{
    public class DenunciasController : Controller
    {
        private readonly AppDbContext _context; 

        public DenunciasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Alta(int registroId)
        {
            ViewBag.RegistroId = registroId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Alta(Denuncia nuevaDenuncia)
        {
            nuevaDenuncia.ID = 0; // Limpiamos ID por seguridad

            if (ModelState.IsValid)
            {
                _context.Denuncias.Add(nuevaDenuncia);
                await _context.SaveChangesAsync();

                TempData["MensajeExito"] = "La denuncia fue registrada correctamente.";
                return RedirectToAction("Detalles", "Registros", new { id = nuevaDenuncia.RegistroId });
            }

            ViewBag.RegistroId = nuevaDenuncia.RegistroId;
            return View(nuevaDenuncia);
        }
    }
}