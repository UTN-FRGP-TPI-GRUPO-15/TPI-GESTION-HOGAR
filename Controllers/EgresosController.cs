using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TPI_GESTION_HOGAR.Datos;
using TPI_GESTION_HOGAR.Models;

namespace TPI_GESTION_HOGAR.Controllers
{
    public class EgresosController : Controller
    {
        private readonly AppDbContext _context;

        public EgresosController(AppDbContext context)
        {
            _context = context;
        }

       
        [HttpGet]
        public async Task<IActionResult> Crear(int registroId)
        {
               var registro = await _context.Registros
                .Include(r => r.Mujer)
                .FirstOrDefaultAsync(r => r.Id == registroId);

            if (registro == null) return NotFound();

            ViewBag.NombreResidente = $"{registro.Mujer?.Apellido}, {registro.Mujer?.Nombre}";
            ViewBag.RegistroId = registroId;

            var nuevoEgreso = new Egreso
            {
                RegistroId = registroId,
                Fecha = DateOnly.FromDateTime(DateTime.Now) 
            };

            return View(nuevoEgreso);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Egreso egreso, bool seRetiraSola)
        {
            if (seRetiraSola)
            {
                egreso.ApellidoRef = null;
                egreso.NombreRef = null;
                egreso.DNIRef = null;
                
            }

            ModelState.Remove("Registro");

            if (ModelState.IsValid)
            {
                _context.Egresos.Add(egreso);

                var registro = await _context.Registros
                    .Include(r => r.Mujer)
                    .FirstOrDefaultAsync(r => r.Id == egreso.RegistroId);
                if (registro != null)
                {
                    

                    if (registro.Mujer != null)
                    {
                        registro.Mujer.estado = false;
                    }
                }


                await _context.SaveChangesAsync();

                TempData["MensajeExito"] = "Egreso registrado correctamente. La habitación ha sido liberada.";

                
                return RedirectToAction("Index", "Home");
            }

            
            return View(egreso);
        }
    }
}
