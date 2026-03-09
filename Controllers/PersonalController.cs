using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TPI_GESTION_HOGAR.Datos;
using TPI_GESTION_HOGAR.DTOs;
using TPI_GESTION_HOGAR.Models;

namespace TPI_GESTION_HOGAR.Controllers
{
    [Authorize(Roles = "Administradora")]
    public class PersonalController : Controller
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<Usuario> _hasher = new();

        public PersonalController(AppDbContext context)
        {
            _context = context;   
        }

        //GET: Personal
        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["NameSortParm"] = sortOrder == "Name" ? "name_desc" : "Name";
            ViewData["LastNameSortParm"] = sortOrder == "LastName" ? "lastname_desc" : "LastName";

            var personal = await _context.Personal.ToListAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                personal = personal.Where(p => p.Apellido.Contains(searchString) || p.Nombre.Contains(searchString) || p.Legajo.ToString().Contains(searchString)).ToList();
            }

            switch (sortOrder)
            {
                case "name_desc":
                    personal = personal.OrderByDescending(p => p.Nombre).ToList();
                    break;
                case "Name":
                    personal = personal.OrderBy(p => p.Nombre).ToList();
                    break;
                case "lastname_desc":
                    personal = personal.OrderByDescending(p => p.Apellido).ToList();
                    break;
                default:
                    personal = personal.OrderBy(p => p.Apellido).ToList();
                    break;
            }

            return View(personal);            
        }
        public async Task<IActionResult> Alta()
        {
            await CargarRoles();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Alta(NuevoPersonalDTO dto)
        {
            try
            {
                if (dto.FechaNac > DateOnly.FromDateTime(DateTime.Today))
                {
                    ModelState.AddModelError("FechaNac", "La fecha de nacimiento no puede ser futura");
                }

                if (!ModelState.IsValid)
                {
                    ViewBag.Message = "Debe completar todos los campos obligatorios";
                    ViewBag.IsError = true;
                    await CargarRoles();
                    return View(dto);
                }

                // Validar que Legajo y DNI no estén en uso
                var personalExistente = await _context.Personal
                                            .Where(p => p.Legajo == dto.Legajo || p.DNI == dto.DNI)
                                            .ToListAsync();

                if (personalExistente.Any())
                {
                    if (personalExistente.Any(p => p.Legajo == dto.Legajo))
                        ModelState.AddModelError("Legajo", "El legajo ya está registrado");

                    if (personalExistente.Any(p => p.DNI == dto.DNI))
                        ModelState.AddModelError("DNI", "El DNI ya está registrado");
                }

                // Validar que NombreUsuario y Email no estén en uso
                var usuarioExistente = await _context.Usuarios
                                            .Where(u => u.NombreUsuario == dto.NombreUsuario || u.Email == dto.Email)
                                            .ToListAsync();

                if (usuarioExistente.Any())
                {
                    if (usuarioExistente.Any(u => u.NombreUsuario == dto.NombreUsuario))
                        ModelState.AddModelError("NombreUsuario", "El nombre de usuario ya está en uso");

                    if (usuarioExistente.Any(u => u.Email == dto.Email))
                        ModelState.AddModelError("Email", "El email ya está en uso");
                }

                if (personalExistente.Any() || usuarioExistente.Any())
                {
                    ViewBag.Message = "Corrija los errores antes de continuar";
                    ViewBag.IsError = true;
                    await CargarRoles();
                    return View(dto);
                }


                // Registrar Personal
                var personal = new Personal
                {
                    Legajo = dto.Legajo,
                    Apellido = dto.Apellido,
                    Nombre = dto.Nombre,
                    DNI = dto.DNI,
                    Nacionalidad = dto.Nacionalidad,
                    FechaNac = dto.FechaNac!.Value,
                    Telefono = dto.Telefono,
                    Domicilio = dto.Domicilio,
                    Provincia = dto.Provincia,
                    Localidad = dto.Localidad,
                    Activo = true
                };

                _context.Personal.Add(personal);

                // Registrar Usuario
                var usuario = new Usuario
                {
                    NombreUsuario = dto.NombreUsuario,
                    Clave = _hasher.HashPassword(null!, dto.ClavePlana),
                    Email = dto.Email,
                    RolId = dto.RolId,
                    Personal = personal
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                ViewBag.Message = "Nuevo personal registrado con éxito.";
                ViewBag.IsError = false;
                ModelState.Clear();
                await CargarRoles();
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error al registrar nuevo personal: " + ex.Message;
                ViewBag.IsError = true;
                await CargarRoles();
                return View(dto);
            }
        }

        private async Task CargarRoles()
        {
            var roles = await _context.Roles.ToListAsync();

            ViewBag.Roles = new SelectList(roles, "Id", "Descripcion");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personal = await _context.Personal
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (personal == null)
            {
                return NotFound();
            }

            var personalDTO = new EditarPersonalDTO
            {
                Legajo = personal.Legajo,
                Apellido = personal.Apellido,
                Nombre = personal.Nombre,
                DNI = personal.DNI,
                Nacionalidad = personal.Nacionalidad,
                FechaNac = personal.FechaNac,
                Telefono = personal.Telefono,
                Domicilio = personal.Domicilio,
                Provincia = personal.Provincia,
                Localidad = personal.Localidad,
            };

            // Podés cargar acá los ViewBag de Provincias o Condiciones si los usás en la vista
            return View(personalDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int legajo, EditarPersonalDTO personalMod)
        {
            if (legajo != personalMod.Legajo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {                
                var personal = new Personal
                {
                    Id = await _context.Personal.Where(p => p.Legajo == legajo).Select(p => p.Id).FirstOrDefaultAsync(),
                    Legajo = personalMod.Legajo,
                    Apellido = personalMod.Apellido,
                    Nombre = personalMod.Nombre,
                    DNI = personalMod.DNI,
                    Nacionalidad = personalMod.Nacionalidad,
                    FechaNac = personalMod.FechaNac!.Value,
                    Telefono = personalMod.Telefono,
                    Domicilio = personalMod.Domicilio,
                    Provincia = personalMod.Provincia,
                    Localidad = personalMod.Localidad,
                    Activo = true                    
                };

                personal.Usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.PersonalId == personal.Id);

                try
                {
                    _context.Personal.Update(personal);
                    await _context.SaveChangesAsync();                                                       

                    TempData["MensajeExito"] = "Los datos del personal se actualizaron correctamente.";

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Personal.Any(p => p.Legajo == legajo))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(personalMod);
        }

        public async Task<IActionResult> AltaBaja(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var personal = await _context.Personal.FirstOrDefaultAsync(p => p.Id == Id);

            if (personal == null)
            {
                return NotFound();
            }

            if(personal.Activo)
            {
                personal.Activo = false;
            }
            else
            {
                personal.Activo = true;
            }

            if (ModelState.IsValid)
            {
                _context.Update(personal);
                await _context.SaveChangesAsync();                
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AgregarTurno(int id)
        {            
            var personal = await _context.Personal.FindAsync(id);

            if (personal == null)
            {
                return NotFound();
            }

            // Cargamos la lista para el menú desplegable
            var tipoTurnos = _context.TipoTurnos.ToList();
            ViewBag.TipoTurnos = new SelectList(tipoTurnos, "Id", "Descripcion");

            // Pasamos los datos del personal a la vista usando ViewBag
            ViewBag.PersonalId = personal.Id;
            ViewBag.PersonalNombre = $"{personal.Nombre} {personal.Apellido}";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarTurno(int PersonalId, int TipoTurnoId, DateTime Fecha)
        {
            if (TipoTurnoId == 0)
            {
                ModelState.AddModelError("", "Debe seleccionar un horario.");
                ViewBag.TipoTurnos = _context.TipoTurnos.ToList();
                ViewBag.PersonalId = PersonalId;
                return View();
            }

            if(Fecha < @DateTime.Today)
            {
                ModelState.AddModelError("", "Debe seleccionar una fecha.");
                ViewBag.TipoTurnos = _context.TipoTurnos.ToList();
                ViewBag.PersonalId = PersonalId;
                return View();
            }

            var turno = new Turno
            {
                PersonalId = PersonalId,
                TipoTurnoId = TipoTurnoId,
                Fecha = DateOnly.FromDateTime(Fecha)
            };

            if (ModelState.IsValid)
            {
                var existeTurno = _context.Turnos.FirstOrDefault(t => t.Fecha == turno.Fecha && t.TipoTurnoId == turno.TipoTurnoId && t.PersonalId == turno.PersonalId);

                if (existeTurno != null)
                {
                    ModelState.AddModelError("", "Ya existe un turno para esa fecha, horario y personal.");

                    var tipoTurnos = _context.TipoTurnos.ToList();
                    ViewBag.TipoTurnos = new SelectList(tipoTurnos, "Id", "Descripcion");

                    var personal = _context.Personal.Select(e => new
                    {
                        Id = e.Id,
                        Name = e.Apellido + ", " + e.Nombre
                    });

                    var listaAlfabetica = personal.OrderBy(item => item.Name).ToList();

                    ViewBag.Personal = new SelectList(listaAlfabetica, "Id", "Name");
                }
                else
                {
                    //bool horario1 = turno.TipoTurnoId == 1;
                    //bool turnoAnteriorHorario3 = _context.Turnos.Any(t => t.PersonalId == turno.PersonalId && t.TipoTurnoId == 3 && t.Fecha == turno.Fecha.AddDays(-1));
                    //bool turnoSiguienteHorario2 = _context.Turnos.Any(t => t.PersonalId == turno.PersonalId && t.TipoTurnoId == 2 && t.Fecha == turno.Fecha);

                    //bool horario1valido = horario1 && !turnoAnteriorHorario3 && !turnoSiguienteHorario2;

                    //bool horario2 = turno.TipoTurnoId == 2;
                    //bool turnoAnteriorHorario1 = _context.Turnos.Any(t => t.PersonalId == turno.PersonalId && t.TipoTurnoId == 1 && t.Fecha == turno.Fecha);
                    //bool turnoSiguienteHorario3 = _context.Turnos.Any(t => t.PersonalId == turno.PersonalId && t.TipoTurnoId == 3 && t.Fecha == turno.Fecha);

                    //bool horario2valido = horario2 && !turnoAnteriorHorario1 && !turnoSiguienteHorario3;

                    //bool horario3 = turno.TipoTurnoId == 3;                    
                    //bool turnoAnteriorHorario2 = _context.Turnos.Any(t => t.PersonalId == turno.PersonalId && t.TipoTurnoId == 2 && t.Fecha == turno.Fecha);
                    //bool turnoSiguienteHorario1 = _context.Turnos.Any(t => t.PersonalId == turno.PersonalId && t.TipoTurnoId == 1 && t.Fecha == turno.Fecha.AddDays(1));

                    //bool horario3valido = horario3 && !turnoAnteriorHorario2 && !turnoSiguienteHorario1;

                    //if (horario1 && horario1valido || horario2 && horario2valido || horario3 && horario3valido)
                    //{
                    //    _context.Add(turno);
                    //    await _context.SaveChangesAsync();
                    //    return RedirectToAction(nameof(Index));
                    //}
                    //else 
                    //{
                    //    ModelState.AddModelError("", "El turno no es válido debido a las restricciones de horarios para el personal. Por favor, elija un horario diferente o revise los turnos adyacentes.");

                    //    var tipoTurnos = _context.TipoTurnos.ToList();
                    //    ViewBag.TipoTurnos = new SelectList(tipoTurnos, "Id", "Descripcion");

                    //    var personal = _context.Personal.Select(e => new
                    //    {
                    //        Id = e.Id,
                    //        Name = e.Apellido + ", " + e.Nombre
                    //    });

                    //    var listaAlfabetica = personal.OrderBy(item => item.Name).ToList();

                    //    ViewBag.Personal = new SelectList(listaAlfabetica, "Id", "Name");
                    //}

                    _context.Add(turno);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Detalles", new { id = PersonalId });
                }
            }

            return View(turno);
        }
        public async Task<IActionResult> Detalles(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Buscamos al personal y traemos su usuario y turnos
            var personal = await _context.Personal
                .Include(p => p.Usuario)
                .Include(p => p.Turnos)
                .ThenInclude(t => t.TipoTurno)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (personal == null)
            {
                return NotFound();
            }

            return View(personal);
        }
    }
}
