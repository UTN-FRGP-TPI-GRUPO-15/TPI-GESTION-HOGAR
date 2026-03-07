using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TPI_GESTION_HOGAR.Datos;
using TPI_GESTION_HOGAR.Models;

namespace TPI_GESTION_HOGAR.Controllers
{
    public class MedidasController : Controller
    {
        private readonly AppDbContext _context;

        public MedidasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Alta(int denunciaId)
        {
            var denuncia = await _context.Denuncias.FindAsync(denunciaId);
            if (denuncia == null) return NotFound();

            ViewBag.DenunciaId = denunciaId;
            ViewBag.RegistroId = denuncia.RegistroId;

           
            ViewBag.TiposMedida = await _context.TiposMedidas.OrderBy(t => t.Descripcion).ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Alta(Medida nuevaMedida, string? nuevaMedidaTexto)
        {
           
            if (nuevaMedida.TipoMedidaId == 0 && !string.IsNullOrWhiteSpace(nuevaMedidaTexto))
            {
               
                var nuevoTipo = new TipoMedida { Descripcion = nuevaMedidaTexto };
                _context.TiposMedidas.Add(nuevoTipo);
                await _context.SaveChangesAsync();

                
                nuevaMedida.TipoMedidaId = nuevoTipo.Id;
            }

            
            ModelState.Remove("TipoMedida");
            ModelState.Remove("Denuncia");

            if (ModelState.IsValid)
            {
                _context.Medidas.Add(nuevaMedida);
                await _context.SaveChangesAsync();

                var denuncia = await _context.Denuncias.FindAsync(nuevaMedida.DenunciaId);
                TempData["MensajeExito"] = "Medida agregada correctamente.";

                return RedirectToAction("Detalles", "Registros", new { id = denuncia.RegistroId });
            }

            
            ViewBag.TiposMedida = await _context.TiposMedidas.OrderBy(t => t.Descripcion).ToListAsync();
            ViewBag.DenunciaId = nuevaMedida.DenunciaId;
            return View(nuevaMedida);
        }
    }
}