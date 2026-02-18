using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TPI_GESTION_HOGAR.Datos;
using TPI_GESTION_HOGAR.Models;

namespace TPI_GESTION_HOGAR.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
