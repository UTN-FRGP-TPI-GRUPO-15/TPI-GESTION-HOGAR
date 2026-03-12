using Microsoft.AspNetCore.Mvc;
using TPI_GESTION_HOGAR.Datos;
using TPI_GESTION_HOGAR.Models;

namespace TPI_GESTION_HOGAR.Controllers
{
    public class RecordatoriosController : Controller
    {
        private readonly AppDbContext _context;

        public RecordatoriosController(AppDbContext context)
        {
            _context = context;
        }

        // GUARDAR NUEVO RECORDATORIO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearDesdeDashboard(Recordatorio nuevoRecordatorio)
        {
            // Nos aseguramos que nazca como "No resuelto"
            nuevoRecordatorio.Resuelto = false;

            // Limpiamos el objeto de navegación para que el ModelState sea válido
            ModelState.Remove("Personal");

            if (ModelState.IsValid)
            {
                _context.Recordatorios.Add(nuevoRecordatorio);
                await _context.SaveChangesAsync();
            }

            // Volvemos al Dashboard sin importar qué pase
            return RedirectToAction("Index", "Home");
        }


        
        [HttpPost]
        public async Task<IActionResult> MarcarResuelto(int id, string observacion, int personalId)
        {
            var recordatorio = await _context.Recordatorios.FindAsync(id);
            if (recordatorio != null)
            {
                // 1. Marcamos la tarea como completada
                recordatorio.Resuelto = true;
                recordatorio.ResultadoObservacion = observacion;

                // 2. MAGIA: Si estaba vinculada a una residente, creamos el Seguimiento automático
                if (recordatorio.RegistroId.HasValue)
                {
                    var nuevoSeguimiento = new Seguimiento
                    {
                        RegistroId = recordatorio.RegistroId.Value,
                        PersonalId = personalId,
                        FechaHora = DateTime.Now,
                        Categoria = "Operativa / Alerta Resuelta", // Categoría automática
                        Descripcion = $"[Alerta Resuelta: {recordatorio.Descripcion}] \nResultado: {observacion}"
                    };

                    _context.Seguimientos.Add(nuevoSeguimiento);
                }

                await _context.SaveChangesAsync();
                TempData["MensajeExito"] = "Tarea resuelta y guardada en el historial.";
            }

            return RedirectToAction("Index", "Home");
        }
    }
}