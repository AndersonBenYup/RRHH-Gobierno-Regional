using System.Globalization;
using Capa_Logica;
using Capa_Logica.Exportacion;
using Capa_Logica.Paginacion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Evo_RRLL.Models.ViewModels;

namespace Proyecto_Evo_RRLL.Controllers;

/// <summary>
/// RF-07, RF-08, RF-09 y RF-11: reporte de vacaciones generadas, pendientes y truncas de todo el personal.
/// Pertenece al módulo Permisos (Rol_Acceso 0502007).
/// </summary>
[Authorize]
public class VacacionesController : Controller
{
    public const int TamanoPagina = 25;

    private readonly VacacionesLogica _vacaciones;
    private readonly Services.EmpleadoServicio _empleados;

    public VacacionesController(VacacionesLogica vacaciones, Services.EmpleadoServicio empleados)
    {
        _vacaciones = vacaciones;
        _empleados = empleados;
    }

    [HttpGet]
    public async Task<IActionResult> Index(DateOnly? corte, int? idEmpleado, bool cesados = false, int pagina = 1, int? tamano = null)
    {
        var fecha = corte ?? DateOnly.FromDateTime(DateTime.Today);
        var empleados = await _empleados.EmpleadosAsync();
        var reporte = await _vacaciones.Generar(fecha, idEmpleado, cesados);
        var filas = Filas(reporte, empleados);

        var tamanoPagina = VentanaPaginas.NormalizarTamano(tamano, TamanoPagina);
        var totalPaginas = Math.Max(1, (int)Math.Ceiling(filas.Count / (double)tamanoPagina));
        pagina = Math.Clamp(pagina, 1, totalPaginas);

        ViewBag.Empleados = empleados;
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Corte = fecha.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        ViewBag.Cesados = cesados;
        ViewBag.SinMotivos = (await _vacaciones.MotivosDeVacaciones()).Count == 0;
        ViewBag.SinFechaDeIngreso = reporte.SinFechaDeIngreso;
        ViewBag.Totales = new VacacionesTotales(
            filas.Sum(f => f.Datos.Resumen.Generadas), filas.Sum(f => f.Datos.Resumen.Gozadas),
            filas.Sum(f => f.Datos.Resumen.Pendientes), filas.Sum(f => f.Datos.Resumen.Truncas));
        ViewBag.Paginador = new PaginadorModelo(pagina, totalPaginas, filas.Count, nameof(Index),
            new Dictionary<string, string?>
            {
                ["corte"] = ViewBag.Corte,
                ["idEmpleado"] = idEmpleado?.ToString(),
                ["cesados"] = cesados ? "true" : null
            }, tamanoPagina, "empleados");

        return View(filas.Skip((pagina - 1) * tamanoPagina).Take(tamanoPagina).ToList());
    }

    /// <summary>RF-11: el reporte completo de vacaciones en CSV, con los mismos filtros de la pantalla.</summary>
    [HttpGet]
    public async Task<IActionResult> Exportar(DateOnly? corte, int? idEmpleado, bool cesados, [FromServices] ExportadorTablas exportador)
    {
        var fecha = corte ?? DateOnly.FromDateTime(DateTime.Today);
        var filas = new List<string?[]>
        {
            new[] { "IdEmpleado", "Empleado", "Estado", "Fecha de ingreso", "Fecha de corte", "Años cumplidos",
                    "Generadas", "Gozadas", "Pendientes", "Truncas" }
        };
        foreach (var f in Filas(await _vacaciones.Generar(fecha, idEmpleado, cesados), await _empleados.EmpleadosAsync()))
        {
            var r = f.Datos.Resumen;
            filas.Add(new string?[]
            {
                f.Datos.IdEmpleado.ToString(CultureInfo.InvariantCulture), f.Empleado, f.Datos.Cesado ? "Cesado" : "Activo",
                r.FechaIngreso.ToString("dd/MM/yyyy"), r.FechaCorte.ToString("dd/MM/yyyy"),
                r.AniosCumplidos.ToString(CultureInfo.InvariantCulture), Dias(r.Generadas), Dias(r.Gozadas),
                Dias(r.Pendientes), Dias(r.Truncas)
            });
        }

        var archivo = exportador.Exportar(filas, $"vacaciones_{fecha:yyyyMMdd}");
        return File(archivo.Contenido, archivo.TipoContenido, archivo.NombreArchivo);
    }

    /// <summary>RF-07 a RF-10: saldo de un empleado con los permisos de vacaciones que descuentan.</summary>
    [HttpGet]
    public async Task<IActionResult> Detalle(int idEmpleado, DateOnly? corte)
    {
        var fecha = corte ?? DateOnly.FromDateTime(DateTime.Today);
        ViewBag.NombreEmpleado = await _empleados.NombreEmpleadoAsync(idEmpleado) ?? $"Empleado {idEmpleado}";
        ViewBag.Corte = fecha.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        return View(await _vacaciones.DeEmpleado(idEmpleado, fecha));
    }

    private static List<VacacionesFila> Filas(ReporteVacaciones reporte, List<(int Id, string Nombre)> empleados)
    {
        var nombres = empleados.ToDictionary(e => e.Id, e => e.Nombre);
        return reporte.Empleados
            .Select(v => new VacacionesFila(nombres.GetValueOrDefault(v.IdEmpleado, $"(Empleado {v.IdEmpleado})"), v))
            .OrderBy(f => f.Empleado, StringComparer.CurrentCultureIgnoreCase)
            .ToList();
    }

    private static string Dias(decimal valor) => VacacionesFormato.Dias(valor);
}
