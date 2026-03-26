using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;
using System.Linq;
using System.Threading.Tasks;
using TPI_GESTION_HOGAR.Datos;
using TPI_GESTION_HOGAR.Models;
using TPI_GESTION_HOGAR.ViewModels;

namespace TPI_GESTION_HOGAR.Controllers
{
    public class ReportesController : Controller
    {
        private readonly AppDbContext _context;

        public ReportesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(ReporteViewModel filtros)
        {
           
            var query = _context.Registros
                .Include(r => r.Mujer)
                    .ThenInclude(m => m.Hijos)
                .Include(r => r.Mujer)
                    .ThenInclude(m => m.Condiciones) 
                .Include(r => r.Habitacion)
                .Include(r => r.Egreso)
                .Include(r => r.Denuncias)
                .AsQueryable();

           
            if (filtros.SoloActivas)
            {
                query = query.Where(r => r.Mujer != null && r.Mujer.Estado == true);
            }

            if (!string.IsNullOrEmpty(filtros.Provincia))
            {
                query = query.Where(r => r.Mujer != null && r.Mujer.Provincia == filtros.Provincia);
            }

            if (!string.IsNullOrEmpty(filtros.Localidad))
            {
                query = query.Where(r => r.Mujer != null && r.Mujer.Localidad.Contains(filtros.Localidad));
            }         
                        
            if (filtros.FechaDesde.HasValue)
            {
                query = query.Where(r => r.Fecha >= filtros.FechaDesde.Value);
            }
            if (filtros.FechaHasta.HasValue)
            {
                query = query.Where(r => r.Fecha <= filtros.FechaHasta.Value);
            }
            if (filtros.SoloDeLaCosta)
            {
                query = query.Where(r => r.Mujer != null && r.Mujer.Localidad != null && (
                    r.Mujer.Localidad.ToLower().Contains("la costa") ||
                    r.Mujer.Localidad.ToLower().Contains("san clemente") ||
                    r.Mujer.Localidad.ToLower().Contains("santa teresita") ||
                    r.Mujer.Localidad.ToLower().Contains("mar de ajó") ||
                    r.Mujer.Localidad.ToLower().Contains("mar del tuyú") ||
                    r.Mujer.Localidad.ToLower().Contains("las toninas") ||
                    r.Mujer.Localidad.ToLower().Contains("nueva atlantis") ||
                    r.Mujer.Localidad.ToLower().Contains("punta medanos") ||
                    r.Mujer.Localidad.ToLower().Contains("costa esmeralda") ||
                    r.Mujer.Localidad.ToLower().Contains("san bernardo") ||
                    r.Mujer.Localidad.ToLower().Contains("lucila del mar") ||
                    r.Mujer.Localidad.ToLower().Contains("aguas verdes")
                ));
            }

            var registrosFiltrados = await query.OrderByDescending(r => r.Fecha).ToListAsync();

           
            if (filtros.CantidadHijosMinima.HasValue)
            {
                registrosFiltrados = registrosFiltrados
                    .Where(r => r.Mujer != null && r.Mujer.Hijos != null && r.Mujer.Hijos.Count >= filtros.CantidadHijosMinima.Value)
                    .ToList();
            }

          
            filtros.Resultados = registrosFiltrados;
            filtros.TotalMujeres = registrosFiltrados.Count;
            filtros.TotalMenores = registrosFiltrados.Sum(r => r.Mujer?.Hijos?.Count ?? 0);

            return View(filtros);
        }
        // MÉTODO PARA GENERAR LOS GRÁFICOS
        public async Task<IActionResult> Graficos(ReporteViewModel filtros)
        {
            // 1. Replicamos la misma consulta base
            var query = _context.Registros
                .Include(r => r.Mujer).ThenInclude(m => m.Hijos)
                .Include(r => r.Mujer).ThenInclude(m => m.Condiciones)
                .Include(r => r.Egreso)
                .AsQueryable();

            // 2. Aplicamos los filtros
            if (filtros.SoloActivas) query = query.Where(r => r.Mujer.Estado == true);
            if (!string.IsNullOrEmpty(filtros.Provincia)) query = query.Where(r => r.Mujer.Provincia == filtros.Provincia);
            if (!string.IsNullOrEmpty(filtros.Localidad)) query = query.Where(r => r.Mujer.Localidad.Contains(filtros.Localidad));
            if (filtros.FechaDesde.HasValue) query = query.Where(r => r.Fecha >= filtros.FechaDesde.Value);
            if (filtros.FechaHasta.HasValue) query = query.Where(r => r.Fecha <= filtros.FechaHasta.Value);
            if (filtros.SoloDeLaCosta)
            {
                query = query.Where(r => r.Mujer.Localidad != null && (
                    r.Mujer.Localidad.ToLower().Contains("la costa") ||
                    r.Mujer.Localidad.ToLower().Contains("san clemente") ||
                    r.Mujer.Localidad.ToLower().Contains("santa teresita") ||
                    r.Mujer.Localidad.ToLower().Contains("mar de ajó") ||
                    r.Mujer.Localidad.ToLower().Contains("mar del tuyú") ||
                    r.Mujer.Localidad.ToLower().Contains("las toninas") ||
                    r.Mujer.Localidad.ToLower().Contains("nueva atlantis") ||
                    r.Mujer.Localidad.ToLower().Contains("punta medanos") ||
                    r.Mujer.Localidad.ToLower().Contains("costa esmeralda") ||
                    r.Mujer.Localidad.ToLower().Contains("san bernardo")
                ));
            }
            var registros = await query.ToListAsync();

            if (filtros.CantidadHijosMinima.HasValue)
                registros = registros.Where(r => r.Mujer?.Hijos?.Count >= filtros.CantidadHijosMinima.Value).ToList();

            // 3. CREAR OBJETO DE ESTADÍSTICAS
            var stats = new EstadisticasViewModel { TotalRegistros = registros.Count };

            if (stats.TotalRegistros > 0)
            {
                var today = DateTime.Today;

                // Promedio Edad Mujeres
                var edadesMujeres = registros.Where(r => r.Mujer != null).Select(r => {
                    var bday = r.Mujer.FechaNac.ToDateTime(TimeOnly.MinValue);
                    int age = today.Year - bday.Year;
                    if (bday.Date > today.AddYears(-age)) age--;
                    return age;
                }).ToList();
                stats.PromedioEdadMujeres = edadesMujeres.Any() ? Math.Round(edadesMujeres.Average(), 1) : 0;

                // Con/Sin Hijos
                stats.MujeresConHijos = registros.Count(r => r.Mujer != null && r.Mujer.Hijos != null && r.Mujer.Hijos.Any());
                stats.MujeresSinHijos = stats.TotalRegistros - stats.MujeresConHijos;

                // Promedio Edad Hijos
                var todosLosHijos = registros.Where(r => r.Mujer != null && r.Mujer.Hijos != null).SelectMany(r => r.Mujer.Hijos).ToList();
                if (todosLosHijos.Any())
                {
                    var edadesHijos = todosLosHijos.Select(h => {
                        var bday = h.FechaNac.ToDateTime(TimeOnly.MinValue);
                        int age = today.Year - bday.Year;
                        if (bday.Date > today.AddYears(-age)) age--;
                        return age;
                    }).ToList();
                    stats.PromedioEdadHijos = Math.Round(edadesHijos.Average(), 1);
                }

                // Promedio Días Estadía
                var diasEstadia = registros.Select(r => {
                    var ingreso = r.Fecha.ToDateTime(TimeOnly.MinValue);
                    var egreso = r.Egreso != null ? r.Egreso.Fecha.ToDateTime(TimeOnly.MinValue) : today;
                    return (egreso - ingreso).Days;
                }).ToList();
                stats.PromedioDiasEstadia = diasEstadia.Any() ? Math.Round(diasEstadia.Average(), 1) : 0;

                // Con/Sin Consumo (Id de condición de adicción = 4)
                stats.ConConsumo = registros.Count(r => r.Mujer != null && r.Mujer.Condiciones != null && r.Mujer.Condiciones.Any(c => c.TipoCondicionId == 4));
                stats.SinConsumo = stats.TotalRegistros - stats.ConConsumo;

                // Localidad (La Costa vs Afuera)
                string[] localidadesCosta = { "La Costa", "San Clemente", "Santa Teresita", "Mar de Ajó", "Mar del Tuyú", "Las Toninas", "San Bernardo" };
                stats.DeLaCosta = registros.Count(r => r.Mujer != null && r.Mujer.Localidad != null && localidadesCosta.Any(lc => r.Mujer.Localidad.Contains(lc, StringComparison.OrdinalIgnoreCase)));
                stats.DeAfuera = stats.TotalRegistros - stats.DeLaCosta;
            }

            return View(stats);
        }
    }
}