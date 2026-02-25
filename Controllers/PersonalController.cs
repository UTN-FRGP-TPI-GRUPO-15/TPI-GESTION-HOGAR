using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TPI_GESTION_HOGAR.DTOs;

namespace TPI_GESTION_HOGAR.Controllers
{
    [Authorize(Roles = "Administradora")]
    public class PersonalController : Controller
    {
        public IActionResult Alta()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Alta(NuevoPersonalDTO nuevoPersonal)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Debe completar todos los campos oblgiatorios.";
                ViewBag.IsError = true;
            }

            ViewBag.Message = "Nuevo personal registrado con éxito.";
            ViewBag.IsError = false;
            return View();
        }
    }
}
