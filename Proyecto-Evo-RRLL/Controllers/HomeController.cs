using Capa_Logica;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Evo_RRLL.Models;
using Proyecto_Evo_RRLL.Models.ViewModels;
using System.Diagnostics;

namespace Proyecto_Evo_RRLL.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly EmpleadoLogica _empleadoData;
        private readonly PersonaLogica _personaData;
        private readonly AsistenciaLogica _asistenciaData;
        private readonly EstructOrganizLogica _estructOrganizData;
private readonly CargoLogica _cargoData;
    private readonly RRHH_FeriadoLogica _feriadoData;
    private readonly EmpleadoAreaActualLogica _areaActualData;

    public HomeController(
        EmpleadoLogica empleadoData,
        PersonaLogica personaData,
        AsistenciaLogica asistenciaData,
        EstructOrganizLogica estructuraOrganizData,
        CargoLogica cargoData,
        RRHH_FeriadoLogica feriadoData,
        EmpleadoAreaActualLogica areaActualData)
    {
        _empleadoData = empleadoData;
        _personaData = personaData;
        _asistenciaData = asistenciaData;
        _estructOrganizData = estructuraOrganizData;
        _cargoData = cargoData;
        _feriadoData = feriadoData;
        _areaActualData = areaActualData;
    }

        public async Task<IActionResult> Index()
        {
            var empleados = await _empleadoData.Listar();
            var estructuras = await _estructOrganizData.Listar();
            var personas = await _personaData.Listar();
            var cargos = await _cargoData.Listar();

            // Empleados por área
            var porArea = (await _areaActualData.Listar())
                .GroupBy(a => a.AreaOrganizacional ?? $"Área {a.idAreaOrganiz}")
                .OrderByDescending(g => g.Count())
                .Take(8);
            var areasNombres = new List<string>();
            var areasCounts = new List<int>();
            foreach (var g in porArea)
            {
                areasNombres.Add(g.Key);
                areasCounts.Add(g.Count());
            }

            // Asistencias en los últimos 7 días (incluido hoy)
            var hoy = DateTime.Today;
            var ultimos7 = new List<string>();
            var asisUltimos7 = new List<int>();
            for (var d = 6; d >= 0; d--)
            {
                var fecha = hoy.AddDays(-d);
                ultimos7.Add(fecha.ToString("dd/MM"));
                asisUltimos7.Add(await _asistenciaData.ContarEntreFechas(fecha, fecha));
            }

            // Empleados por estado
            var estadosNombres = new List<string>();
            var estadosCounts = new List<int>();
            foreach (var g in empleados.GroupBy(e => e.Estado switch { true => "Activo", false => "Inactivo", null => "Sin estado" }).OrderByDescending(x => x.Count()))
            {
                estadosNombres.Add(g.Key);
                estadosCounts.Add(g.Count());
            }

            var model = new DashboardViewModel
            {
                Empleados = empleados.Count,
                EmpleadosActivos = empleados.Count(e => e.Estado == true),
                Personas = personas.Count,
                asistenciasHoy = await _asistenciaData.ContarEntreFechas(hoy, hoy),
                Areas = estructuras.Count,
                Cargos = cargos.Count,
                AreasNombres = areasNombres,
                EmpleadosPorArea = areasCounts,
                DiasAsistencia = ultimos7,
                AsistenciasPorDia = asisUltimos7,
                EstadosNombres = estadosNombres,
                EmpleadosPorEstado = estadosCounts,
                FeriadosMes = await FeriadosDelMesAsync(hoy)
            };
            return View(model);
        }

        private async Task<List<string>> FeriadosDelMesAsync(DateTime hoy)
        {
            try
            {
                return (await _feriadoData.Listar())
                    .Where(f => f.Fecha?.Year == hoy.Year && f.Fecha!.Value.Month == hoy.Month)
                    .OrderBy(f => f.Fecha)
                    .Select(f => string.IsNullOrWhiteSpace(f.Motivo) ? $"{f.Fecha:dd/MM}" : $"{f.Fecha:dd/MM} - {f.Motivo}")
                    .ToList();
            }
            catch
            {
                return new List<string>();
            }
        }

        public IActionResult Ayuda()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult PaginaNoEncontrada(int statusCode = 404)
        {
            return View(statusCode);
        }

        public IActionResult Privacidad()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}