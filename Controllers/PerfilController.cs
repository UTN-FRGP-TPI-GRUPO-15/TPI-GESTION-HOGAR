using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TPI_GESTION_HOGAR.Datos;
using TPI_GESTION_HOGAR.Models;
using TPI_GESTION_HOGAR.ViewModel.Perfil;

namespace TPI_GESTION_HOGAR.Controllers
{
    [Authorize]
    public class PerfilController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PerfilController> _logger;

        public PerfilController(AppDbContext context, ILogger<PerfilController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var usuario = await GetUsuarioActualAsync();

            if (usuario == null)
                return NotFound();

            _logger.LogInformation("Usuario encontrado: {NombreUsuario}", usuario.NombreUsuario);

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

        public async Task<IActionResult> Editar()
        {
            var usuario = await GetUsuarioActualAsync();

            if (usuario == null)
                return NotFound();

            var viewModel = new EditarPerfilViewModel
            {
                Nombre = usuario.Personal.Nombre,
                Apellido = usuario.Personal.Apellido,
                Nacionalidad = usuario.Personal.Nacionalidad,
                FechaNac = usuario.Personal.FechaNac,
                Telefono = usuario.Personal.Telefono,
                Domicilio = usuario.Personal.Domicilio,
                Localidad = usuario.Personal.Localidad,
                Email = usuario.Email
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(EditarPerfilViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            try
            {
                var usuario = await GetUsuarioActualAsync();

                if (usuario == null)
                    return NotFound();

                if (!string.IsNullOrEmpty(viewModel.PasswordNueva))
                {
                    if (string.IsNullOrEmpty(viewModel.PasswordActual))
                    {
                        ModelState.AddModelError("PasswordActual", "Debes ingresar tu contraseña actual para establecer una nueva.");
                        return View(viewModel);
                    }

                    // Verificamos usando el hasher de ASP.NET Core
                    bool passwordCorrecta = VerificarPassword(usuario, viewModel.PasswordActual);

                    if (!passwordCorrecta)
                    {
                        ModelState.AddModelError("PasswordActual", "La contraseña actual es incorrecta.");
                        return View(viewModel);
                    }

                    // Guardamos la nueva contraseña hasheada en la propiedad 'Clave'
                    usuario.Clave = EncriptarPassword(usuario, viewModel.PasswordNueva);
                }

                // Actualizar datos personales y de contacto
                usuario.Personal.Nombre = viewModel.Nombre;
                usuario.Personal.Apellido = viewModel.Apellido;
                usuario.Personal.Nacionalidad = viewModel.Nacionalidad;
                usuario.Personal.FechaNac = viewModel.FechaNac;
                usuario.Personal.Telefono = viewModel.Telefono;
                usuario.Personal.Domicilio = viewModel.Domicilio;
                usuario.Personal.Localidad = viewModel.Localidad;
                usuario.Email = viewModel.Email;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Perfil actualizado para el usuario: {NombreUsuario}", usuario.NombreUsuario);
                TempData["MensajeExito"] = "Perfil actualizado correctamente.";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el perfil para el usuario: {NombreUsuario}", User.Identity?.Name);
                TempData["MensajeError"] = "Ocurrió un error al actualizar el perfil. Por favor, inténtalo nuevamente.";
                return View(viewModel);
            }
        }

        private async Task<Usuario?> GetUsuarioActualAsync()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
                return null;

            return await _context.Usuarios
                            .Where(u => u.Id == userId)
                            .Include(u => u.Personal)
                            .FirstOrDefaultAsync();
        }

        private readonly PasswordHasher<Usuario> _hasher = new();

        private bool VerificarPassword(Usuario usuario, string passwordIngresada)
        {
            var resultado = _hasher.VerifyHashedPassword(usuario, usuario.Clave, passwordIngresada);
            return resultado == PasswordVerificationResult.Success;
        }

        private string EncriptarPassword(Usuario usuario, string nuevaPassword)
        {
            return _hasher.HashPassword(usuario, nuevaPassword);
        }
    }
}