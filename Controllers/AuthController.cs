using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TPI_GESTION_HOGAR.Datos;
using TPI_GESTION_HOGAR.Models;
using TPI_GESTION_HOGAR.Services;

namespace TPI_GESTION_HOGAR.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<Usuario> _hasher = new();
        private readonly IEmailService _emailService;

        public AuthController(AppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string usuario, string password)
        {
            try
            {
                var user = _context.Usuarios
                    .Include(u => u.Rol)
                    .FirstOrDefault(u => u.NombreUsuario == usuario);

                if (user == null)
                    return BadLogin();

                var resultado = _hasher.VerifyHashedPassword(user, user.Clave, password);

                if (resultado != PasswordVerificationResult.Success)
                    return BadLogin();

                await SignInUser(user);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                //Para testing, si hay un error dejar que pase al front
                ViewBag.Message = ex.Message;
                ViewBag.IsError = true;
                return View();
            }
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var resetLink = Url.Action("ResetPassword", "Auth", new { token = "token_placeholder" }, Request.Scheme);

            await _emailService.SendPasswordResetAsync(email, resetLink ?? "token_placeholder_null");

            ViewBag.EmailEnviado = true;

            return View();
        }

        public IActionResult ResetPassword(string token)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Auth");
        }

        public IActionResult Denied()
        {
            return View();
        }

        private IActionResult BadLogin()
        {
            ViewBag.Message = "Credenciales incorrectas";
            ViewBag.IsError = true;
            return View();
        }

        private async Task SignInUser(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, usuario.NombreUsuario),
                new(ClaimTypes.Role, usuario.Rol.Descripcion)
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);
        }
    }
}
