using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TPI_GESTION_HOGAR.Models;
using TPI_GESTION_HOGAR.Datos;

namespace TPI_GESTION_HOGAR.Controllers
{
    public class MujeresController : Controller
    {
        private readonly AppDbContext _context;

        public MujeresController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string buscarTexto)
        {
           
            var query = _context.Mujeres.Where(m => m.estado == true);

           
            if (!string.IsNullOrEmpty(buscarTexto))
            {
                
                query = query.Where(m =>
                    m.DNI.ToString().Contains(buscarTexto) ||
                    m.Nombre.Contains(buscarTexto) ||
                    m.Apellido.Contains(buscarTexto));
            }

            var mujeres = await query.ToListAsync();

           
            ViewData["FiltroTexto"] = buscarTexto;

            return View(mujeres);
        }

        [HttpGet]
        public IActionResult VerificarDni()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerificarDni(int dniBuscado)
        {
          
            if (dniBuscado < 1000000 || dniBuscado > 99999999)
            {
              
                ModelState.AddModelError("dniBuscado", "Por favor, ingrese un número de DNI válido (entre 7 y 8 dígitos, sin puntos).");
                return View();
            }

           
            var mujerExistente = await _context.Mujeres.FirstOrDefaultAsync(m => m.DNI == dniBuscado);

            if (mujerExistente != null)
            {
                TempData["MensajeExito"] = "La residente ya se encuentra registrada en el sistema.";
                return RedirectToAction("Editar", new { id = mujerExistente.ID });
            }
            else
            {
                return RedirectToAction("AltaMujer", new { dniVerificado = dniBuscado });
            }
        
        }
        [HttpGet]
        public IActionResult AltaMujer(int? dniVerificado)
        {
            ViewBag.TipoCondiciones = _context.TipoCondiciones.ToList();

            var nuevaMujer = new Mujer();
            if (dniVerificado.HasValue)
            {
                nuevaMujer.DNI = dniVerificado.Value;
            }

            return View(nuevaMujer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AltaMujer(Mujer nuevaMujer, string Provincia, int? TipoCondicionId, string? ObservacionesCondicion)
        {
            if (ModelState.IsValid)
            {
                nuevaMujer.estado = true;

                if (!string.IsNullOrEmpty(Provincia))
                {
                    nuevaMujer.Localidad = $"{nuevaMujer.Localidad}, {Provincia}";
                }

                _context.Mujeres.Add(nuevaMujer);
                await _context.SaveChangesAsync();

                if (TipoCondicionId.HasValue)
                {
                    var nuevaObservacion = new ObservacionCondicion
                    {
                        Descripcion = ObservacionesCondicion ?? "Sin observaciones adicionales"
                    };

                    _context.Add(nuevaObservacion);
                    await _context.SaveChangesAsync();

                    var nuevaCondicion = new Condicion
                    {
                        MujerId = nuevaMujer.ID,
                        TipoCondicionId = TipoCondicionId.Value,
                        ObservacionCondicionId = nuevaObservacion.Id
                    };

                    _context.Condiciones.Add(nuevaCondicion);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.TipoCondiciones = _context.TipoCondiciones.ToList();
            return View(nuevaMujer);
        }
    }
}
