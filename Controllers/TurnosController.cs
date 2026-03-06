using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TPI_GESTION_HOGAR.Datos;
using TPI_GESTION_HOGAR.DTOs;
using TPI_GESTION_HOGAR.Models;

namespace TPI_GESTION_HOGAR.Controllers
{
    public class TurnosController : Controller
    {
        private readonly AppDbContext _context;

        public TurnosController(AppDbContext context)
        {
            _context = context;
        }

        //GET: Turnos
        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            ViewData["CurrentFilter"] = searchString; 
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["NameSortParm"] = sortOrder == "Name" ? "name_desc" : "Name";

            var turnos = await _context.Turnos
                .Include(t => t.TipoTurno)
                .Include(t => t.Personal)
                .ToListAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                turnos = turnos.Where(t => t.Personal.Apellido.Contains(searchString) || t.Personal.Nombre.Contains(searchString)).ToList();
            }

            switch (sortOrder)
            {
                case "date_desc":
                    turnos = turnos.OrderByDescending(t => t.Fecha).ToList();
                    break;
                case "Name":
                    turnos = turnos.OrderBy(t => t.Personal.Apellido).ToList();
                    break;
                case "name_desc":
                    turnos = turnos.OrderByDescending(t => t.Personal.Apellido).ToList();
                    break;
                default:
                    turnos = turnos.OrderBy(t => t.Fecha).ToList();
                    break;
            }

            return View(turnos);
        }

        //GET: Turnos/History
        public async Task<IActionResult> History(string searchString, string sortOrder)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["NameSortParm"] = sortOrder == "Name" ? "name_desc" : "Name";

            var turnos = await _context.Turnos
                .Include(t => t.TipoTurno)
                .Include(t => t.Personal)
                .ToListAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                turnos = turnos.Where(t => t.Personal.Apellido.Contains(searchString) || t.Personal.Nombre.Contains(searchString)).ToList();
            }

            switch (sortOrder)
            {
                case "date_desc":
                    turnos = turnos.OrderByDescending(t => t.Fecha).ToList();
                    break;
                case "Name":
                    turnos = turnos.OrderBy(t => t.Personal.Apellido).ToList();
                    break;
                case "name_desc":
                    turnos = turnos.OrderByDescending(t => t.Personal.Apellido).ToList();
                    break;
                default:
                    turnos = turnos.OrderBy(t => t.Fecha).ToList();
                    break;
            }
                       
            return View(turnos);
        }

        //GET: Turnos/Create

        public IActionResult Create()
        {
            var tipoTurnos = _context.TipoTurnos.ToList();
            ViewBag.TipoTurnos = new SelectList(tipoTurnos, "Id", "Descripcion");

            var personal = _context.Personal.Select(e => new
            {
                Id = e.Id,
                Name = e.Apellido + ", " + e.Nombre
            });

            var listaAlfabetica = personal.OrderBy(item => item.Name).ToList();

            ViewBag.Personal = new SelectList(listaAlfabetica, "Id", "Name");

            return View();
        }

        //POST: Turnos/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Turno turno)
        {
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
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(turno);
        }

        //GET: Turnos/Edit

        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var turno = await _context.Turnos
                .Include(t => t.TipoTurno)
                .Include(t => t.Personal)
                .FirstOrDefaultAsync(t => t.ID == Id);

            if (turno == null)
            {
                return NotFound();
            }

            var tipoTurnos = _context.TipoTurnos.ToList();
            ViewBag.TipoTurnos = new SelectList(tipoTurnos, "Id", "Descripcion");

            var personal = _context.Personal.Select(e => new
            {
                Id = e.Id,
                Name = e.Apellido + ", " + e.Nombre
            });

            var listaAlfabetica = personal.OrderBy(item => item.Name).ToList();

            ViewBag.Personal = new SelectList(listaAlfabetica, "Id", "Name");

            return View(turno);
        }

        //POST: Turnos/Edit

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, Turno turno)
        {
            if(Id != turno.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(turno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(turno);
        }

        //GET: Turnos/Delete

        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var turno = await _context.Turnos
                .Include(t => t.TipoTurno)
                .Include(t => t.Personal)
                .FirstOrDefaultAsync(t => t.ID == Id);

            if (turno == null)
            {
                return NotFound();
            }
                      
            return View(turno);
        }

        //POST: Turnos/Delete

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int Id)
        {
            var turno = await _context.Turnos.FindAsync(Id);

            if (turno != null)
            {
                _context.Turnos.Remove(turno);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Planificacion(DateOnly? fecha)
        {
            DateOnly hoy = DateOnly.FromDateTime(DateTime.Today);

            DateOnly fechaBuscada = fecha ?? hoy;

            int diferencia = fechaBuscada.DayOfWeek == DayOfWeek.Sunday ? 6 : (int)fechaBuscada.DayOfWeek - 1;

            DateOnly inicioSemana = fechaBuscada.AddDays(-diferencia);
            DateOnly finSemana = inicioSemana.AddDays(6);

            ViewBag.Turnos = await ObtenerTurnos(inicioSemana, finSemana);
            ViewBag.Personal = await ObtenerPersonal();
            ViewBag.TipoTurnos = await ObtenerTipoTurnos();
            ViewBag.InicioSemana = inicioSemana;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GuardarPlanificacion(List<NuevoTurnoDTO> turnos)
        {
            var fechas = turnos.Select(t => t.Fecha).Distinct();

            var turnosExistentes = await _context.Turnos
                                    .Where(t => fechas.Contains(t.Fecha))
                                    .ToListAsync();

            foreach (var dto in turnos)
            {
                var turnoExistente = turnosExistentes.FirstOrDefault(t => t.Fecha == dto.Fecha && t.TipoTurnoId == dto.TipoTurnoId);

                if (turnoExistente != null)
                {
                    if (dto.PersonalId != null)
                        turnoExistente.PersonalId = dto.PersonalId.Value;

                    else
                        _context.Turnos.Remove(turnoExistente);
                }
                else if (dto.PersonalId != null)
                {
                    var nuevoTurno = new Turno
                    {
                        Fecha = dto.Fecha,
                        TipoTurnoId = dto.TipoTurnoId,
                        PersonalId = dto.PersonalId.Value
                    };

                    _context.Turnos.Add(nuevoTurno);
                }
            }

            await _context.SaveChangesAsync();

            TempData["MensajeExito"] = "Planificación guardada correctamente";

            return RedirectToAction("Planificacion");
        }

        private async Task<List<Turno>> ObtenerTurnos(DateOnly fechaInicio, DateOnly fechaFin)
        {
            List<Turno> turnos = await _context.Turnos
                                    .Where(t => t.Fecha >= fechaInicio && t.Fecha <= fechaFin.AddDays(6))
                                    .ToListAsync();

            return turnos;
        }

        private async Task<List<Personal>> ObtenerPersonal()
        {
            List<Personal> personal = await _context.Personal.ToListAsync();

            return personal;
        }

        private async Task<List<TipoTurno>> ObtenerTipoTurnos()
        {
            List<TipoTurno> tipoTurnos = await _context.TipoTurnos.ToListAsync();

            return tipoTurnos;
        }
    }
}
