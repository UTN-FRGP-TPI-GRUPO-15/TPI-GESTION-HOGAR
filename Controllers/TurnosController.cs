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
            ViewBag.TipoTurnoId = new SelectList(_context.TipoTurnos, "Id", "Descripcion");
            ViewBag.PersonalId = new SelectList(_context.Personal, "Id", "Nombre", "Apellido");

            return View();
        }
    }
}
