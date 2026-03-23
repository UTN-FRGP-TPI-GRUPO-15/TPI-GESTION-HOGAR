using Microsoft.AspNetCore.Mvc;
using TPI_GESTION_HOGAR.Datos;
using TPI_GESTION_HOGAR.Models;

namespace TPI_GESTION_HOGAR.Controllers
{
    public class SeguimientosController : Controller
    {
        private readonly AppDbContext _context;

        public SeguimientosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CargaRapida(int registroId, string categoria, int personalId, string descripcion, IFormFile archivoAdjunto, string tipoDocumento)
        {
            var nuevaNovedad = new Seguimiento
            {
                RegistroId = registroId,
                PersonalId = personalId,
                Categoria = categoria,
                Descripcion = descripcion,
                FechaHora = DateTime.Now 
            };

            _context.Seguimientos.Add(nuevaNovedad);
            await _context.SaveChangesAsync();
            if (archivoAdjunto != null && archivoAdjunto.Length > 0)
            {
                string rutaCarpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "documentos");
                if (!Directory.Exists(rutaCarpeta)) Directory.CreateDirectory(rutaCarpeta);

                string nombreUnico = Guid.NewGuid().ToString() + "_" + archivoAdjunto.FileName;
                string rutaFisica = Path.Combine(rutaCarpeta, nombreUnico);

                using (var stream = new FileStream(rutaFisica, FileMode.Create))
                {
                    await archivoAdjunto.CopyToAsync(stream);
                }

                
                var nuevoDoc = new Documento
                {
                    NombreArchivo = archivoAdjunto.FileName,
                    RutaArchivo = "/uploads/documentos/" + nombreUnico,
                    TipoDocumento = string.IsNullOrEmpty(tipoDocumento) ? "Otro" : tipoDocumento, // Tomamos lo que eligió la operadora
                    FechaSubida = DateTime.Now,
                    RegistroId = registroId
                };

                _context.Documentos.Add(nuevoDoc);
                await _context.SaveChangesAsync();
            }

            TempData["MensajeExito"] = "Novedad guardada exitosamente en el legajo.";

            
            return RedirectToAction("Index", "Home");
        }
    }
}