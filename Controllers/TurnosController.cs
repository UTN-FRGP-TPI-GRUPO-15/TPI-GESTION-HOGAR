using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public async Task<IActionResult> Index()
        {
            var turnos = await _context.Turnos
                .Include(t => t.TipoTurno)
                .Include(t => t.Personal)
                .ToListAsync();

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
                Name = e.Nombre + " " + e.Apellido
            });

            ViewBag.Personal = new SelectList(personal, "ID", "Name");

            return View();
        }
    }
}
