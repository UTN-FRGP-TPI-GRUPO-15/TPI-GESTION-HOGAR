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
        private readonly ILogger<AuthController> _logger;

        public AuthController(AppDbContext context, IEmailService emailService, ILogger<AuthController> logger)
        {
            _context = context;
            _emailService = emailService;
            _logger = logger;
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
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

            if (user != null)
            {
                _logger.LogInformation("Usuario encontrado para {EmailUsuario}", email);

                user.ResetToken = Guid.NewGuid().ToString();
                user.ResetTokenExpiry = DateTime.Now.AddHours(1);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Token generado para {EmailUsuario}: {ResetToken}", email, user.ResetToken);
                _logger.LogInformation("Token expirará el {ExpiryTime} para {EmailUsuario}", user.ResetTokenExpiry, email);
                _logger.LogInformation("Hora actual: {CurrentTime}", DateTime.Now);

                var resetLink = Url.Action("ResetPassword", "Auth", new { token = user.ResetToken }, Request.Scheme);

                await _emailService.SendPasswordResetAsync(email, resetLink ?? "null_token");
            }
            else
            {
                _logger.LogWarning("No se encontró un usuario para {EmailUsuario}", email);
                await Task.Delay(2000);
            }

            ViewBag.EmailEnviado = true;
            return View();
        }

        public IActionResult ResetPassword(string token)
        {
            var user = _context.Usuarios.FirstOrDefault(u => u.ResetToken == token && u.ResetTokenExpiry > DateTime.Now);

            ViewBag.TokenValido = user != null;
            ViewBag.Token = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string token, string newPassword)
        {
            var user = _context.Usuarios.FirstOrDefault(u => u.ResetToken == token && u.ResetTokenExpiry > DateTime.Now);

            if (user == null)
            {
                ViewBag.TokenValido = false;
                return View();
            }

            user.ResetToken = null;
            user.ResetTokenExpiry = null;
            user.Clave = _hasher.HashPassword(user, newPassword);

            await _context.SaveChangesAsync();

            ViewBag.TokenValido = true;
            ViewBag.Token = token;
            ViewBag.PasswordReset = true;

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
                new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
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
