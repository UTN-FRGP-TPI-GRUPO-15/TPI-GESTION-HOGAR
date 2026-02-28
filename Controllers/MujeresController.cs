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

        [HttpGet]
        public async Task<IActionResult> VerCondicion(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

           
            var mujer = await _context.Mujeres
                .Include(m => m.Condiciones)
                .ThenInclude(c => c.TipoCondicion) 
                .Include(m => m.Condiciones)
                .ThenInclude(c => c.ObservacionCondicion)
                .Include(m => m.Hijos)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (mujer == null)
            {
                return NotFound();
            }

            return View(mujer);
        }
        [HttpGet]
        public async Task<IActionResult> AgregarCondicion(int id) // Recibe el ID de la Mujer
        {
            // Buscamos a la mujer para poder mostrar su nombre en la pantalla
            var mujer = await _context.Mujeres.FindAsync(id);
            if (mujer == null)
            {
                return NotFound();
            }

            // Cargamos la lista para el menú desplegable
            ViewBag.TipoCondiciones = await _context.TipoCondiciones.ToListAsync();

            // Pasamos los datos de la mujer a la vista usando ViewBag
            ViewBag.MujerId = mujer.ID;
            ViewBag.MujerNombre = $"{mujer.Nombre} {mujer.Apellido}";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarCondicion(int MujerId, int TipoCondicionId, string? ObservacionesCondicion)
        {
            // 1. Validamos que haya seleccionado una condición
            if (TipoCondicionId == 0)
            {
                ModelState.AddModelError("", "Debe seleccionar un tipo de condición.");
                ViewBag.TipoCondiciones = _context.TipoCondiciones.ToList();
                ViewBag.MujerId = MujerId;
                return View();
            }

            // 2. Creamos y guardamos la observación
            var nuevaObservacion = new ObservacionCondicion
            {
                Descripcion = !string.IsNullOrWhiteSpace(ObservacionesCondicion) ? ObservacionesCondicion : "Sin observaciones adicionales"
            };

            _context.Add(nuevaObservacion);
            await _context.SaveChangesAsync();

            // 3. Creamos el puente (Condición) uniendo a la Mujer, el Tipo y la Observación
            var nuevaCondicion = new Condicion
            {
                MujerId = MujerId,
                TipoCondicionId = TipoCondicionId,
                ObservacionCondicionId = nuevaObservacion.Id
            };

            _context.Condiciones.Add(nuevaCondicion);
            await _context.SaveChangesAsync();

            // 4. Cartel de éxito y volvemos a la ficha de la residente
            TempData["MensajeExito"] = "Nueva condición agregada correctamente al historial.";
            return RedirectToAction("VerCondicion", new { id = MujerId });
        }
        [HttpGet]
        public async Task<IActionResult> Detalles(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Buscamos a la residente y traemos todo su historial médico adjunto
            var mujer = await _context.Mujeres
                .Include(m => m.Condiciones)
                .ThenInclude(c => c.TipoCondicion)
                .Include(m => m.Condiciones)
                .ThenInclude(c => c.ObservacionCondicion)
                .Include(m => m.Hijos)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (mujer == null)
            {
                return NotFound();
            }

            return View(mujer);
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

               
                return RedirectToAction("Detalles", new { id = nuevoHijo.MujerId });
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

            // Buscamos a la residente por su ID
            var mujer = await _context.Mujeres.FindAsync(id);
            if (mujer == null) return NotFound();

            // La mandamos a la pantalla. Como ya tiene datos, los casilleros aparecerán llenos.
            return View(mujer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Mujer mujerModificada)
        {
            if (id != mujerModificada.ID) return NotFound();

            if (ModelState.IsValid)
            {
                // 1. Buscamos a la residente original
                var mujerOriginal = await _context.Mujeres.FindAsync(id);
                if (mujerOriginal == null) return NotFound();

                // 2. Actualizamos los campos
                mujerOriginal.DNI = mujerModificada.DNI;
                mujerOriginal.Nombre = mujerModificada.Nombre;
                mujerOriginal.Apellido = mujerModificada.Apellido;
                mujerOriginal.FechaNac = mujerModificada.FechaNac;
                mujerOriginal.Nacionalidad = mujerModificada.Nacionalidad;
                mujerOriginal.Genero = mujerModificada.Genero;
                mujerOriginal.NivelEducativo = mujerModificada.NivelEducativo;
                mujerOriginal.Ocupacion = mujerModificada.Ocupacion;
                mujerOriginal.Telefono = mujerModificada.Telefono;
                mujerOriginal.Domicilio = mujerModificada.Domicilio;
                mujerOriginal.Localidad = mujerModificada.Localidad;

               
                // 3. Guardamos los cambios
                await _context.SaveChangesAsync();
                TempData["MensajeExito"] = "Los datos de la residente se actualizaron correctamente.";

                return RedirectToAction("Detalles", new { id = mujerOriginal.ID });
            }

            // Si algo falla, recarga la vista
            return View(mujerModificada);
        }





    }
}
