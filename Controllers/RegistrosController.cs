using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TPI_GESTION_HOGAR.Datos;
using TPI_GESTION_HOGAR.Models;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using System.IO;


namespace TPI_GESTION_HOGAR.Controllers
{
    public class RegistrosController : Controller
    {
        private readonly AppDbContext _context;

        public RegistrosController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [HttpPost]
        [ValidateAntiForgeryToken]

      
        public async Task<IActionResult> ImportarHistorial(IFormFile archivoExcel)
        {
            if (archivoExcel == null || archivoExcel.Length == 0)
            {
                TempData["MensajeError"] = "Por favor, seleccione un archivo Excel válido.";
                return RedirectToAction("Index", "Home");
            }

            using (var stream = new MemoryStream())
            {
                await archivoExcel.CopyToAsync(stream);
                using (var workbook = new XLWorkbook(stream))
                {
                    var worksheet = workbook.Worksheet(1); // Lee la primera pestaña ("Ingresos")
                    var rows = worksheet.RangeUsed().RowsUsed().Skip(1); // Saltea los encabezados

                    int casosImportados = 0;

                    using (var transaction = await _context.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            foreach (var row in rows)
                            {
                                // 1. DNI: Leemos el valor directo y limpiamos
                                string dniStr = row.Cell(4).Value.ToString().Replace(".", "").Trim();
                                if (!int.TryParse(dniStr, out int dniLimpio)) continue;

                                var mujerExistente = await _context.Mujeres.FirstOrDefaultAsync(m => m.DNI == dniLimpio);
                                int mujerId;

                                if (mujerExistente == null)
                                {
                                    // 2. NOMBRE Y APELLIDO
                                    string nombreCompleto = row.Cell(2).Value.ToString().Trim();
                                    var partesNombre = nombreCompleto.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                                    string apellido = partesNombre.Length > 0 ? partesNombre[0] : "Desconocido";
                                    string nombre = partesNombre.Length > 1 ? string.Join(" ", partesNombre.Skip(1)) : "Sin Nombre";

                                    // 3. EDAD Y FECHA DE NACIMIENTO
                                    string edadStr = row.Cell(5).Value.ToString().Trim();
                                    if (!int.TryParse(edadStr, out int edadCalculada) || edadCalculada < 15 || edadCalculada > 100)
                                    {
                                        edadCalculada = 30; // 30 años por defecto si la celda está vacía o tiene texto raro
                                    }
                                    var fechaNac = new DateOnly(DateTime.Today.Year - edadCalculada, 1, 1);

                                    // 4. CREAR MUJER
                                    var nuevaMujer = new Mujer
                                    {
                                        DNI = dniLimpio,
                                        Apellido = apellido,
                                        Nombre = nombre,
                                        Nacionalidad = "Argentina",
                                        FechaNac = fechaNac,
                                        Telefono = row.Cell(8).Value.ToString(),
                                        Localidad = row.Cell(18).Value.ToString(),
                                        Estado = false // Inactiva (Historial)
                                    };

                                    _context.Mujeres.Add(nuevaMujer);
                                    await _context.SaveChangesAsync();
                                    mujerId = nuevaMujer.ID;
                                }
                                else
                                {
                                    mujerId = mujerExistente.ID;
                                }

                                // 5. FECHA DE INGRESO (Extracción segura)
                                DateTime fechaIngresoDt = DateTime.Today; // Por defecto hoy
                                var celdaFecha = row.Cell(1);

                                if (celdaFecha.DataType == XLDataType.DateTime)
                                {
                                    fechaIngresoDt = celdaFecha.GetDateTime();
                                }
                                else if (DateTime.TryParse(celdaFecha.Value.ToString(), out DateTime fechaParseada))
                                {
                                    fechaIngresoDt = fechaParseada;
                                }

                                // 6. CREAR REGISTRO
                                var nuevoRegistro = new Registro
                                {
                                    Fecha = DateOnly.FromDateTime(fechaIngresoDt),
                                    Estado = false,
                                    MujerID = mujerId,
                                    HabitacionId = null
                                };

                                _context.Registros.Add(nuevoRegistro);
                                await _context.SaveChangesAsync();
                                // 7. CREAR EGRESO (Si el caso ya terminó)
                                string celdaFechaEgreso = row.Cell(11).Value.ToString().Trim();

                                if (!string.IsNullOrEmpty(celdaFechaEgreso) && celdaFechaEgreso.ToLower() != "en curso")
                                {
                                    DateTime fechaEgresoDt = DateTime.Today;
                                    var celdaEgreso = row.Cell(11);

                                    if (celdaEgreso.DataType == XLDataType.DateTime)
                                    {
                                        fechaEgresoDt = celdaEgreso.GetDateTime();
                                    }
                                    else if (DateTime.TryParse(celdaEgreso.Value.ToString(), out DateTime fechaParseada))
                                    {
                                        fechaEgresoDt = fechaParseada;
                                    }

                                    var nuevoEgreso = new Egreso
                                    {
                                        RegistroId = nuevoRegistro.Id,
                                        Fecha = DateOnly.FromDateTime(fechaEgresoDt),

                                        // Usamos las propiedades reales de tu modelo
                                        DomicilioRef = row.Cell(12).Value.ToString().Trim(),
                                        NombreRef = row.Cell(13).Value.ToString().Trim() // La col 13 es "Referente de egreso"
                                    };

                                    _context.Egresos.Add(nuevoEgreso);
                                }
                                casosImportados++;


                            }


                            await _context.SaveChangesAsync();
                            await transaction.CommitAsync();

                            TempData["MensajeExito"] = $"Se importaron {casosImportados} casos históricos correctamente.";
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            TempData["MensajeError"] = "Error al procesar el archivo. Detalle: " + ex.Message;
                        }
                    }
                }
            }

            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Detalles(int? id)
        {
            if (id == null) return NotFound();

            var registro = await _context.Registros
                .Include(r => r.Mujer)
                    .ThenInclude(m => m.Hijos)
                    .Include(r => r.Mujer)
                        .ThenInclude(m => m.Condiciones)
                            .ThenInclude(c => c.TipoCondicion)
                 .Include(r => r.Mujer)
                    .ThenInclude(m => m.Condiciones)
                        .ThenInclude(c => c.ObservacionCondicion)
                .Include(r => r.Habitacion)        
                .Include(r => r.Agresores)         
                .Include(r => r.Denuncias)
                .ThenInclude(d => d.Medida)
                .ThenInclude(m => m.TipoMedida)

                .FirstOrDefaultAsync(r => r.Id == id);

            if (registro == null) return NotFound();

            return View(registro);
        }
        [HttpGet]
        public async Task<IActionResult> Legajo(int? id)
        {

            if (id == null) return NotFound();

            var registro = await _context.Registros
                .Include(r => r.Mujer)
                .Include(r => r.Seguimientos)
                    .ThenInclude(s => s.Personal)
                .Include(r => r.Documentos)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (registro == null) return NotFound();
            return View(registro);
        }

        [HttpGet]
        public async Task<IActionResult> AsignarHabitacion(int? id)
        {
            if (id == null) return NotFound();

     
            var registro = await _context.Registros
                .Include(r => r.Mujer)
                    .ThenInclude(m => m.Hijos)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (registro == null) return NotFound();

            
            int camasNecesarias = 1 + (registro.Mujer?.Hijos?.Count ?? 0);
            var habitacionesActivas = await _context.Habitaciones
                .Include(h => h.Registros.Where(r => r.Mujer != null && r.Mujer.Estado == true))
                .Where(h => h.Estado == true)
                .ToListAsync();

            var listaHabitaciones = habitacionesActivas
                .Where(h =>
                 
                    h.Id == registro.HabitacionId ||
                   
                    (!h.Registros.Any() && h.Capacidad >= camasNecesarias)
                )
                .Select(h => new SelectListItem
                {
                    Value = h.Id.ToString(),
                    Text = $"Habitación {h.NroHabitacion} - (Capacidad total: {h.Capacidad} plazas)",
                    Selected = h.Id == registro.HabitacionId
                }).ToList();

            ViewBag.Habitaciones = listaHabitaciones;
            ViewBag.CamasNecesarias = camasNecesarias;

            return View(registro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AsignarHabitacion(int id, int? habitacionId)
        {
            var registro = await _context.Registros.FindAsync(id);
            if (registro == null || registro.Id != id) return NotFound();

            // Actualizamos la habitación (Si elige "Sin asignar", habitacionId viaja como null y es válido gracias a tu migración)
            registro.HabitacionId = habitacionId;

            await _context.SaveChangesAsync();

            TempData["MensajeExito"] = "La asignación de cama se actualizó correctamente.";

            // Al terminar, la devolvemos al Dashboard principal
            return RedirectToAction("Index", "Home");
        }

        
    }
}
