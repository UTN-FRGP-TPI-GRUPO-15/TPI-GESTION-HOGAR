using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TPI_GESTION_HOGAR.Datos;
using TPI_GESTION_HOGAR.Models;

namespace TPI_GESTION_HOGAR.Controllers
{
    public class AgresoresController : Controller
    {
        private readonly AppDbContext _context; 

        public AgresoresController(AppDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public IActionResult Alta(int registroId)
        {
            // Recibimos el ID del Registro/Expediente al que pertenece
            ViewBag.RegistroId = registroId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Alta(Agresor nuevoAgresor)
        {
            // Limpiamos el ID por seguridad (como hicimos con los hijos)
            nuevoAgresor.ID = 0;

            if (ModelState.IsValid)
            {
                _context.Agresores.Add(nuevoAgresor);
                await _context.SaveChangesAsync();
                TempData["MensajeExito"] = "Los datos del agresor fueron registrados en el expediente.";

                // Volvemos a la vista de Detalles del Registro/Expediente
                return RedirectToAction("Detalles", "Registros", new { id = nuevoAgresor.RegistroId });
            }

            ViewBag.RegistroId = nuevoAgresor.RegistroId;
            return View(nuevoAgresor);
        }

        
        [HttpGet]
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null) return NotFound();

            var agresor = await _context.Agresores.FindAsync(id);
            if (agresor == null) return NotFound();

            return View(agresor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Agresor agresorModificado)
        {
            if (id != agresorModificado.ID) return NotFound();

            if (ModelState.IsValid)
            {
                var agresorOriginal = await _context.Agresores.FindAsync(id);
                if (agresorOriginal == null) return NotFound();

                // Pisamos SOLO los campos permitidos
                agresorOriginal.DNI = agresorModificado.DNI;
                agresorOriginal.Nombre = agresorModificado.Nombre;
                agresorOriginal.Apellido = agresorModificado.Apellido;
                agresorOriginal.Vinculo = agresorModificado.Vinculo;
                agresorOriginal.FechaNac = agresorModificado.FechaNac;
                agresorOriginal.Nacionalidad = agresorModificado.Nacionalidad;
                agresorOriginal.Genero = agresorModificado.Genero;
                agresorOriginal.NivelEducativo = agresorModificado.NivelEducativo;
                agresorOriginal.Ocupacion = agresorModificado.Ocupacion;
                agresorOriginal.Telefono = agresorModificado.Telefono;
                agresorOriginal.Domicilio = agresorModificado.Domicilio;
                agresorOriginal.Localidad = agresorModificado.Localidad;

                await _context.SaveChangesAsync();
                TempData["MensajeExito"] = "Los datos del agresor se actualizaron correctamente.";

                return RedirectToAction("Detalles", "Registros", new { id = agresorOriginal.RegistroId });
            }

            return View(agresorModificado);
        }
    }
}