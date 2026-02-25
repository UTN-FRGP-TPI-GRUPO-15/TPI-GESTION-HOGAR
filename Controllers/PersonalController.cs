using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TPI_GESTION_HOGAR.Datos;
using TPI_GESTION_HOGAR.DTOs;

namespace TPI_GESTION_HOGAR.Controllers
{
    [Authorize(Roles = "Administradora")]
    public class PersonalController : Controller
    {
        private readonly AppDbContext _context;

        public PersonalController(AppDbContext context)
        {
            _context = context;   
        }

        public async Task<IActionResult> Alta()
        {
            var roles = await _context.Roles.ToListAsync();

            ViewBag.Roles = new SelectList(roles, "Id", "Descripcion");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Alta(NuevoPersonalDTO nuevoPersonal)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Debe completar todos los campos oblgiatorios.";
                ViewBag.IsError = true;
                return View();
            }

            ViewBag.Message = "Nuevo personal registrado con éxito.";
            ViewBag.IsError = false;
            return View();
        }
    }
}
