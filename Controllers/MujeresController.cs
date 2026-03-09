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
              
                ModelState.AddModelError("dniBuscado", "Ingrese un dni valido");
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
                var primerIngreso = new Registro
                {
                    Fecha = DateOnly.FromDateTime(DateTime.Today), 
                    Estado = true,                                 
                    MujerID = nuevaMujer.ID                        
                                                                   
                };

                _context.Registros.Add(primerIngreso);
                await _context.SaveChangesAsync();
               
                return RedirectToAction("Index", "Home");
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


        // ==========================================
        // EDITAR DATOS Y/O GENERAR RE-INGRESO
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mujer = await _context.Mujeres.FindAsync(id);
            if (mujer == null)
            {
                return NotFound();
            }

            // Podés cargar acá los ViewBag de Provincias o Condiciones si los usás en la vista
            return View(mujer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Mujer mujerModificada, bool generarIngreso = false)
        {
            if (id != mujerModificada.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   
                    _context.Update(mujerModificada);
                    await _context.SaveChangesAsync();

                    
                    if (generarIngreso)
                    {
                        var nuevoIngreso = new Registro
                        {
                            Fecha = DateOnly.FromDateTime(DateTime.Today), 
                            Estado = true,                                 
                            MujerID = mujerModificada.ID                   
                                                                           
                        };

                        _context.Registros.Add(nuevoIngreso);
                        await _context.SaveChangesAsync();

                        TempData["MensajeExito"] = "Los datos de la residente fueron actualizados y se registró su nuevo ingreso al hogar.";

                     
                        return RedirectToAction("Index", "Home");
                    }

                   
                    TempData["MensajeExito"] = "Los datos de la residente se actualizaron correctamente.";
                    return RedirectToAction(nameof(Index)); 
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MujerExists(mujerModificada.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

         
            return View(mujerModificada);
        }

       
        private bool MujerExists(int id)
        {
            return _context.Mujeres.Any(e => e.ID == id);
        }





    }
}
