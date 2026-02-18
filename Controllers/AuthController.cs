using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TPI_GESTION_HOGAR.Datos;
using TPI_GESTION_HOGAR.Models;

namespace TPI_GESTION_HOGAR.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<Usuario> _hasher = new();

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string usuario, string password)
        {
            var user = _context.Usuarios.FirstOrDefault(u => u.NombreUsuario == usuario && u.Clave == password);

            if (user == null)
            {
                ViewBag.Message = "Credenciales incorrectas";
                ViewBag.IsError = true;
                return View();
            }

            ViewBag.Message = "Sesión iniciada correctamente";
            ViewBag.IsError = false;
            return View();
        }
    }
}
