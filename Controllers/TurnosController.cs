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
        private readonly Dictionary<int, int> _horasTipoTurno = new()
        {
            { 1, 6 },
            { 2, 6 },
            { 3, 12 }
        };

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
                turnos = turnos.Where(
                    t => t.Personal.Apellido.ToLower().Contains(searchString.ToLower()) || 
                    t.Personal.Nombre.ToLower().Contains(searchString.ToLower())).
                    ToList();
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
                turnos = turnos.Where(
                    t => t.Personal.Apellido.ToLower().Contains(searchString.ToLower()) || 
                    t.Personal.Nombre.ToLower().Contains(searchString.ToLower()))
                    .ToList();
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

            var personal = _context.Personal.Where(p => p.Activo).Select(p => new
            {
                Id = p.Id,
                Name = p.Apellido + ", " + p.Nombre
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

        public async Task<IActionResult> Planificacion(DateOnly? fecha, bool repetir = false)
        {
            DateOnly hoy = DateOnly.FromDateTime(DateTime.Today);

            DateOnly fechaBuscada = fecha ?? hoy;

            await CargarPlanificacionView(fechaBuscada, null, repetir);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GuardarPlanificacion(List<NuevoTurnoDTO> turnos, DateOnly fecha)
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);

            var fechas = turnos.Select(t => t.Fecha).Distinct();

            var turnosExistentes = await _context.Turnos
                                    .Where(t => fechas.Contains(t.Fecha))
                                    .ToListAsync();

            foreach (var dto in turnos)
            {
                var turnoExistente = turnosExistentes
                    .FirstOrDefault(t => t.Fecha == dto.Fecha && t.TipoTurnoId == dto.TipoTurnoId);

                // Cortar si hay cambios en un turno pasado, sino pasar a siguiente iteración
                if (dto.Fecha < hoy)
                {
                    bool huboCambio =
                        (turnoExistente == null && dto.PersonalId != null) || // Crea
                        (turnoExistente != null && dto.PersonalId == null) || // Borra
                        (turnoExistente != null && dto.PersonalId != turnoExistente.PersonalId) ||
                        (turnoExistente != null && dto.PersonalOpcId != turnoExistente.PersonalOpcId);

                    if (huboCambio)
                    {
                        TempData["MensajeError"] = "No se pueden modificar turnos pasados.";

                        await CargarPlanificacionView(fecha, turnos);

                        return View("Planificacion");
                    }

                    continue;
                }

                var error = await ValidarTurnos(turnos);

                if (error != null)
                {
                    TempData["MensajeError"] = error;
                    await CargarPlanificacionView(fecha, turnos);
                    return View("Planificacion");
                }

                if (turnoExistente != null)
                {
                    if (dto.PersonalId != null)
                    {
                        turnoExistente.PersonalId = dto.PersonalId.Value;
                        turnoExistente.PersonalOpcId = dto.PersonalOpcId;
                    }

                    else
                        _context.Turnos.Remove(turnoExistente);
                }
                else if (dto.PersonalId != null)
                {
                    var nuevoTurno = new Turno
                    {
                        Fecha = dto.Fecha,
                        TipoTurnoId = dto.TipoTurnoId,
                        PersonalId = dto.PersonalId.Value,
                        PersonalOpcId = dto.PersonalOpcId
                    };

                    _context.Turnos.Add(nuevoTurno);
                }
            }

            await _context.SaveChangesAsync();

            TempData["MensajeExito"] = "Planificación guardada correctamente";

            return RedirectToAction("Planificacion", new { fecha });
        }

        private async Task<string?> ValidarTurnos(List<NuevoTurnoDTO> turnos)
        {
            string errorHtml = "<ul>";
            int contadorErrores = 0;

            // 1. Verificar que no haya una operadora con más de un turno el mismo día
            bool hayDuplicado = turnos
                .Where(t => t.PersonalId != null)
                .GroupBy(t => new { t.Fecha, t.PersonalId })
                .Any(g => g.Count() > 1);

            if (hayDuplicado)
            {
                contadorErrores++;
                errorHtml += "<li>No se puede asignar a la misma operadora más de un turno por día.</li>";
            }

            // 2. Validar no más de 12 horas continuas (noche -> mañana)
            var turnosOrdenados = turnos
                .Where(t => t.PersonalId != null)
                .OrderBy(t => t.PersonalId)
                .ThenBy(t => t.Fecha)
                .ToList();

            for (int i = 0; i < turnosOrdenados.Count - 1; i++)
            {
                var actual = turnosOrdenados[i];
                var siguiente = turnosOrdenados[i + 1];

                if (actual.PersonalId != siguiente.PersonalId)
                    continue;

                if (actual.TipoTurnoId == 3 &&
                    siguiente.TipoTurnoId == 1 &&
                    siguiente.Fecha == actual.Fecha.AddDays(1))
                {
                    contadorErrores++;
                    errorHtml += "<li>No se puede asignar turno mañana luego de un turno noche para la misma operadora.</li>";
                    break;
                }
            }

            // 3. Validar carga horaria semanal máxima
            var horasPorOperadora = turnos
                .Where(t => t.PersonalId != null)
                .GroupBy(t => t.PersonalId!.Value)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(t => _horasTipoTurno[t.TipoTurnoId])
                );

            var valor = await _context.Configuracion
                .Where(c => c.Clave == "MaxHorasSemanalesOperadora")
                .Select(c => c.Valor)
                .FirstOrDefaultAsync();

            int maxHoras = int.TryParse(valor, out int resultado) ? resultado : 48;

            var excedidas = horasPorOperadora
                .Where(kv => kv.Value > maxHoras)
                .Select(kv => kv.Key)
                .ToList();

            if (excedidas.Any())
            {
                contadorErrores++;

                var nombres = await _context.Personal
                    .Where(p => excedidas.Contains(p.Id))
                    .Select(p => p.Apellido + ", " + p.Nombre)
                    .ToListAsync();

                var listaHtml = "<ul>" + string.Join("", nombres.Select(n => $"<li>{n}</li>")) + "</ul>";

                errorHtml += $"<li>Las siguientes operadoras exceden la carga horaria semanal máxima de {maxHoras} horas:{listaHtml}</li>";
            }

            return contadorErrores > 0 ? errorHtml += "</ul>" : null;
        }

        private async Task CargarPlanificacionView(DateOnly fecha, List<NuevoTurnoDTO>? turnos = null, bool repetir = false)
        {
            int diferencia = fecha.DayOfWeek == DayOfWeek.Sunday ? 6 : (int)fecha.DayOfWeek - 1;

            DateOnly inicioSemana = fecha.AddDays(-diferencia);
            DateOnly finSemana = inicioSemana.AddDays(6);

            List<NuevoTurnoDTO> resultado;

            if (!repetir)
                resultado = turnos ?? await ObtenerTurnosDTO(inicioSemana, finSemana);
            else
            {
                var hoy = DateOnly.FromDateTime(DateTime.Today);

                var turnosSemanaActual = await ObtenerTurnosDTO(inicioSemana, finSemana);
                var turnosSemanaAnterior = await ObtenerTurnosDTO(inicioSemana.AddDays(-7), finSemana.AddDays(-7));

                var repetidos = turnosSemanaAnterior.Select(t => new NuevoTurnoDTO
                {
                    Fecha = t.Fecha.AddDays(7),
                    TipoTurnoId = t.TipoTurnoId,
                    PersonalId = t.PersonalId,
                    PersonalOpcId = t.PersonalOpcId
                })
                .Where(t => t.Fecha >= hoy)
                .ToList();

                foreach (var rep in repetidos)
                {
                    var existe = turnosSemanaActual.FirstOrDefault(a =>
                        a.Fecha == rep.Fecha &&
                        a.TipoTurnoId == rep.TipoTurnoId
                    );

                    if (existe == null || existe.PersonalId == null)
                    {
                        turnosSemanaActual.RemoveAll(a =>
                            a.Fecha == rep.Fecha &&
                            a.TipoTurnoId == rep.TipoTurnoId
                        );

                        turnosSemanaActual.Add(rep);
                    }
                }

                resultado = turnosSemanaActual;
            }

            ViewBag.Turnos = resultado;
            ViewBag.Operadores = await ObtenerOperadores();
            ViewBag.TipoTurnos = await ObtenerTipoTurnos();
            ViewBag.InicioSemana = inicioSemana;
        }

        private async Task<List<NuevoTurnoDTO>> ObtenerTurnosDTO(DateOnly fechaInicio, DateOnly fechaFin)
        {
            List<Turno> turnos = await _context.Turnos
                                    .Where(t => t.Fecha >= fechaInicio && t.Fecha <= fechaFin.AddDays(6))
                                    .ToListAsync();

            return turnos.Select(t => new NuevoTurnoDTO
            {
                Fecha = t.Fecha,
                TipoTurnoId = t.TipoTurnoId,
                PersonalId = t.PersonalId,
                PersonalOpcId = t.PersonalOpcId
            }).ToList();
        }

        private async Task<List<Personal>> ObtenerOperadores()
        {
            List<Personal> personal = await _context.Personal
                .Where(p => p.Activo && p.Usuario.Rol.Descripcion == "Operadora")
                .ToListAsync();

            return personal;
        }

        private async Task<List<TipoTurno>> ObtenerTipoTurnos()
        {
            List<TipoTurno> tipoTurnos = await _context.TipoTurnos.ToListAsync();

            return tipoTurnos;
        }
    }
}
