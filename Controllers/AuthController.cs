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
            try
            {
                var user = _context.Usuarios.FirstOrDefault(u => u.NombreUsuario == usuario);

                if (user == null)
                    return BadLogin();

                var resultado = _hasher.VerifyHashedPassword(user, user.Clave, password);

                if (resultado != PasswordVerificationResult.Success)
                    return BadLogin();

                ViewBag.Message = "Sesión iniciada correctamente";
                ViewBag.IsError = false;
                return View();
            }
            catch (Exception ex)
            {
                //Para testing, si hay un error dejar que pase al front
                ViewBag.Message = ex.Message;
                ViewBag.IsError = true;
                return View();
            }
        }

        private IActionResult BadLogin()
        {
            ViewBag.Message = "Credenciales incorrectas";
            ViewBag.IsError = true;
            return View();
        }
    }
}
