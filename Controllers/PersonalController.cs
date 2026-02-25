using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TPI_GESTION_HOGAR.Datos;
using TPI_GESTION_HOGAR.DTOs;
using TPI_GESTION_HOGAR.Models;

namespace TPI_GESTION_HOGAR.Controllers
{
    [Authorize(Roles = "Administradora")]
    public class PersonalController : Controller
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<Usuario> _hasher = new();

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

                // Validar que Legajo y DNI no estén en uso
                var personalExistente = await _context.Personal
                                            .Where(p => p.Legajo == dto.Legajo || p.DNI == dto.DNI)
                                            .ToListAsync();

                if (personalExistente.Any())
                {
                    if (personalExistente.Any(p => p.Legajo == dto.Legajo))
                        ModelState.AddModelError("Legajo", "El legajo ya está registrado");

                    if (personalExistente.Any(p => p.DNI == dto.DNI))
                        ModelState.AddModelError("DNI", "El DNI ya está registrado");
                }

                // Validar que NombreUsuario y Email no estén en uso
                var usuarioExistente = await _context.Usuarios
                                            .Where(u => u.NombreUsuario == dto.NombreUsuario || u.Email == dto.Email)
                                            .ToListAsync();

                if (usuarioExistente.Any())
                {
                    if (usuarioExistente.Any(u => u.NombreUsuario == dto.NombreUsuario))
                        ModelState.AddModelError("NombreUsuario", "El nombre de usuario ya está en uso");

                    if (usuarioExistente.Any(u => u.Email == dto.Email))
                        ModelState.AddModelError("Email", "El email ya está en uso");
                }

                if (personalExistente.Any() || usuarioExistente.Any())
                {
                    ViewBag.Message = "Corrija los errores antes de continuar";
                    ViewBag.IsError = true;
                    await CargarRoles();
                    return View(dto);
                }


                // Registrar Personal
                var personal = new Personal
                {
                    Legajo = dto.Legajo,
                    Apellido = dto.Apellido,
                    Nombre = dto.Nombre,
                    DNI = dto.DNI,
                    Nacionalidad = dto.Nacionalidad,
                    FechaNac = dto.FechaNac!.Value,
                    Telefono = dto.Telefono,
                    Domicilio = dto.Domicilio,
                    Localidad = dto.Localidad,
                    Activo = true
                };

                _context.Personal.Add(personal);

                // Registrar Usuario
                var usuario = new Usuario
                {
                    NombreUsuario = dto.NombreUsuario,
                    Clave = _hasher.HashPassword(null!, dto.ClavePlana),
                    Email = dto.Email,
                    RolId = dto.RolId,
                    Personal = personal
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

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
