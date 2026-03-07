using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TPI_GESTION_HOGAR.Datos;
using TPI_GESTION_HOGAR.Models; 

namespace TPI_GESTION_HOGAR.Controllers
{
    public class HijosController : Controller
    {
        private readonly AppDbContext _context;

        public HijosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> AgregarHijo(int id)
        {
            var mujer = await _context.Mujeres.FindAsync(id);
            if (mujer == null) return NotFound();

            ViewBag.MujerId = mujer.ID;
            ViewBag.MujerNombre = $"{mujer.Nombre} {mujer.Apellido}";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarHijo(Hijo nuevoHijo)
        {
            
            nuevoHijo.ID = 0;

            if (ModelState.IsValid)
            {
                _context.Hijos.Add(nuevoHijo);
                await _context.SaveChangesAsync();
                TempData["MensajeExito"] = "Registro estadístico guardado correctamente.";

                
                return View(nuevoHijo);
            }

            var mujer = await _context.Mujeres.FindAsync(nuevoHijo.MujerId);
            ViewBag.MujerId = mujer.ID;
            ViewBag.MujerNombre = $"{mujer.Nombre} {mujer.Apellido}";
            return View(nuevoHijo);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null) return NotFound();

            // Buscamos al menor en la base de datos
            var hijo = await _context.Hijos.FindAsync(id);
            if (hijo == null) return NotFound();

            return View(hijo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Hijo hijoModificado)
        {
            if (id != hijoModificado.ID) return NotFound();

            if (ModelState.IsValid)
            {
                // 1. Buscamos el registro original
                var hijoOriginal = await _context.Hijos.FindAsync(id);
                if (hijoOriginal == null) return NotFound();

                // 2. 
                hijoOriginal.DNI = hijoModificado.DNI;
                hijoOriginal.Nombre = hijoModificado.Nombre;
                hijoOriginal.Apellido = hijoModificado.Apellido;
                hijoOriginal.FechaNac = hijoModificado.FechaNac;

                // 3. Guardamos
                await _context.SaveChangesAsync();
                TempData["MensajeExito"] = "Los datos del menor se actualizaron correctamente.";

                // 4.Volvemos a la Ficha Técnica de la MADRE
                return RedirectToAction("Detalles", "Mujeres", new { id = hijoOriginal.MujerId });
            }

            return View(hijoModificado);
        }




    }
}