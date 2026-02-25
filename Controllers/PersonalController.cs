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
    }
}
