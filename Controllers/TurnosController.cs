using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TPI_GESTION_HOGAR.Datos;
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
                ID = e.ID,
                Name = e.Apellido + ", " + e.Nombre
            });

            var listaAlfabetica = personal.OrderBy(item => item.Name).ToList();

            ViewBag.Personal = new SelectList(listaAlfabetica, "ID", "Name");

            return View();
        }

        //POST: Turnos/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Turno turno)
        {
            if (ModelState.IsValid)
            {
                _context.Add(turno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
                ID = e.ID,
                Name = e.Apellido + ", " + e.Nombre
            });

            var listaAlfabetica = personal.OrderBy(item => item.Name).ToList();

            ViewBag.Personal = new SelectList(listaAlfabetica, "ID", "Name");

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
    }
}
