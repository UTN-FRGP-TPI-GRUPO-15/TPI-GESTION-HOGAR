using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TPI_GESTION_HOGAR.Datos;
using TPI_GESTION_HOGAR.DTOs;
using TPI_GESTION_HOGAR.ViewModel.Perfil;

namespace TPI_GESTION_HOGAR.Controllers
{
    [Authorize]
    public class PerfilController : Controller
    {
        private readonly AppDbContext _context;

        public PerfilController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var usuario = await _context.Usuarios
                            .Where(u => u.Id == userId)
                            .Include(u => u.Personal)
                            .FirstOrDefaultAsync();

            if (usuario == null)
            {
                return NotFound();
            }

            var viewModel = new MiPerfilViewModel
            {
                Nombre = usuario.Personal.Nombre,
                Apellido = usuario.Personal.Apellido,
                Legajo = usuario.Personal.Legajo,
                DNI = usuario.Personal.DNI,
                Nacionalidad = usuario.Personal.Nacionalidad,
                FechaNac = usuario.Personal.FechaNac,
                Telefono = usuario.Personal.Telefono,
                Domicilio = usuario.Personal.Domicilio,
                Localidad = usuario.Personal.Localidad,

                Email = usuario.Email,
                NombreUsuario = usuario.NombreUsuario
            };

            return View(viewModel);
        }

        public IActionResult Editar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Editar(EditarPerfilDTO dto)
        {
            return View();
        }

        //private Task<IActionResult> ObtenerDatosUsuario()

        public IActionResult GuardarCambios()
        {
            // Lógica para guardar los cambios del perfil
            return RedirectToAction("Index");
        }
    }
}
