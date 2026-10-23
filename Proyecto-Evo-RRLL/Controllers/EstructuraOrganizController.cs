using Capa_Logica;
using Capa_Datos;
using Capa_Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Evo_RRLL.Models.ViewModels;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class EstructuraOrganizController : Controller
{
    private readonly EstructOrganizLogica _areaData;
    private readonly ResponsableXUOLogica _responsableData;
    private readonly Empleado_AreaLogica _empleadoAreaData;
    private readonly LocalLogica _localData;
    private readonly EmpleadoLogica _empleadoData;
    private readonly PersonaLogica _personaData;
    private readonly EstructOrganizDotacionLogica _dotacionData;
    private readonly SecuenciaService _secuencia;
    private readonly Services.EmpleadoServicio _empleados;

    public EstructuraOrganizController(
        EstructOrganizLogica areaData,
        ResponsableXUOLogica responsableData,
        Empleado_AreaLogica empleadoAreaData,
        LocalLogica localData,
        EmpleadoLogica empleadoData,
        PersonaLogica personaData,
        EstructOrganizDotacionLogica dotacionData,
        SecuenciaService secuencia,
        Services.EmpleadoServicio empleados)
    {
        _areaData = areaData;
        _responsableData = responsableData;
        _empleadoAreaData = empleadoAreaData;
        _localData = localData;
        _empleadoData = empleadoData;
        _personaData = personaData;
        _dotacionData = dotacionData;
        _secuencia = secuencia;
        _empleados = empleados;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? year)
    {
        var areas = await _areaData.Listar();
        var anios = areas.Select(a => a.Year).Distinct().OrderByDescending(x => x).ToList();
        var anio = year ?? anios.FirstOrDefault();
        var empleados = await EmpleadosAsync();
        var dotacion = (await _dotacionData.Listar()).Where(d => d.Year == anio).ToDictionary(d => d.idAreaOrganiz);

        var lista = areas
            .Where(a => a.Year == anio)
            .OrderBy(a => a.idAreaOrganiz)
            .Select(a => new AreaOrganizFila
            {
                Year = a.Year,
                IdAreaOrganiz = a.idAreaOrganiz,
                AreaOrganizacional = a.AreaOrganizacional,
                Abrev = a.Abrev,
                Sigla = a.Sigla,
                DependeDe = areas.FirstOrDefault(d => d.Year == a.YearDe && d.idAreaOrganiz == a.idDependeDe)?.AreaOrganizacional,
                Jefe = a.IdEmpleado is null ? null : empleados.FirstOrDefault(e => e.Id == a.IdEmpleado).Nombre,
                NroTrabajaUO = dotacion.GetValueOrDefault(a.idAreaOrganiz)?.NroTrabajaUO
            })
            .ToList();

        ViewBag.Anios = anios;
        ViewBag.Anio = anio;
        return View(lista);
    }

    [HttpGet]
    public async Task<IActionResult> Nuevo(int year)
    {
        await CargarCatalogos(year);
        return View("Form", new EstructOrganiz { Year = year });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Nuevo(EstructOrganiz modelo)
    {
        if (string.IsNullOrWhiteSpace(modelo.AreaOrganizacional))
            ModelState.AddModelError(nameof(modelo.AreaOrganizacional), "El nombre del área es obligatorio.");

        if (ModelState.IsValid)
        {
            var guardo = await _secuencia.EjecutarConBloqueoAsync(
                async () =>
                {
                    var porAnio = (await _areaData.Listar()).Where(a => a.Year == modelo.Year).ToList();
                    return porAnio.Count > 0 ? porAnio.Max(a => a.idAreaOrganiz) + 1 : 1;
                },
                async id =>
                {
                    modelo.idAreaOrganiz = id;
                    modelo.Abrev ??= string.Empty;
                    modelo.Sigla ??= string.Empty;
                    await _areaData.Crear(modelo);
                    return true;
                });
            if (guardo)
            {
                TempData["MensajeExito"] = "Unidad orgánica registrada.";
                return RedirectToAction(nameof(Index), new { year = modelo.Year });
            }
            ModelState.AddModelError("", "No se pudo registrar la unidad orgánica: intente nuevamente.");
        }

        await CargarCatalogos(modelo.Year);
        return View("Form", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int year, int idAreaOrganiz)
    {
        var area = await _areaData.Obtener(year, idAreaOrganiz);
        if (area is null)
            return NotFound();

        await CargarCatalogos(year);
        return View("Form", area);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(EstructOrganiz modelo)
    {
        var actual = await _areaData.Obtener(modelo.Year, modelo.idAreaOrganiz);
        if (actual is null)
            return NotFound();

        actual.AreaOrganizacional = modelo.AreaOrganizacional;
        actual.Abrev = modelo.Abrev ?? string.Empty;
        actual.Sigla = modelo.Sigla ?? string.Empty;
        actual.Mision = modelo.Mision;
        actual.Vision = modelo.Vision;
        actual.YearDe = modelo.YearDe;
        actual.idDependeDe = modelo.idDependeDe;
        actual.IdEmpleado = modelo.IdEmpleado;
        await _areaData.Actualizar(actual);

        TempData["MensajeExito"] = "Unidad orgánica actualizada.";
        return RedirectToAction(nameof(Index), new { year = modelo.Year });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int year, int idAreaOrganiz)
    {
        var actual = await _areaData.Obtener(year, idAreaOrganiz);
        if (actual is not null)
            await _areaData.Eliminar(actual);
        return RedirectToAction(nameof(Index), new { year });
    }

    [HttpGet]
    public async Task<IActionResult> Detalles(int year, int idAreaOrganiz)
    {
        var area = await _areaData.Obtener(year, idAreaOrganiz);
        if (area is null)
            return NotFound();

        var areas = await _areaData.Listar();
        var empleados = await EmpleadosAsync();
        var asignaciones = (await _empleadoAreaData.Listar())
            .Where(ea => ea.Year == year && ea.idAreaOrganiz == idAreaOrganiz)
            .ToList();
        var responsables = (await _responsableData.Listar())
            .Where(r => r.Year == year && r.idAreaOrganiz == idAreaOrganiz)
            .ToList();

        ViewBag.Area = area;
        ViewBag.NroTrabajaUO = (await _dotacionData.Obtener(year, idAreaOrganiz))?.NroTrabajaUO;
        ViewBag.DependeDe = areas.FirstOrDefault(d => d.Year == area.YearDe && d.idAreaOrganiz == area.idDependeDe)?.AreaOrganizacional;
        ViewBag.Jefe = area.IdEmpleado is null ? null : empleados.FirstOrDefault(e => e.Id == area.IdEmpleado).Nombre;
        ViewBag.Empleados = empleados;
        ViewBag.Asignaciones = asignaciones;
        ViewBag.Responsables = responsables;
        ViewBag.ResponsableData = _responsableData;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AgregarEmpleado(int year, int idAreaOrganiz, int idEmpleado)
    {
        if (await _areaData.Obtener(year, idAreaOrganiz) is null)
            return NotFound();

        var existente = await _empleadoAreaData.Obtener(year, idAreaOrganiz, idEmpleado);
        if (existente is null)
            await _empleadoAreaData.Crear(new Empleado_Area { Year = year, idAreaOrganiz = idAreaOrganiz, IdEmpleado = idEmpleado });

        return RedirectToAction(nameof(Detalles), new { year, idAreaOrganiz });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> QuitarEmpleado(int year, int idAreaOrganiz, int idEmpleado)
    {
        var existente = await _empleadoAreaData.Obtener(year, idAreaOrganiz, idEmpleado);
        if (existente is not null)
            await _empleadoAreaData.Eliminar(existente);
        return RedirectToAction(nameof(Detalles), new { year, idAreaOrganiz });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResponsableNuevo(ResponsableXUO modelo)
    {
        modelo.IdResponsable = 0;
        await _responsableData.Crear(modelo);
        TempData["MensajeExito"] = "Responsable asignado.";
        return RedirectToAction(nameof(Detalles), new { year = modelo.Year, idAreaOrganiz = modelo.idAreaOrganiz });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResponsableEliminar(int idResponsable)
    {
        var actual = await _responsableData.Obtener(idResponsable);
        if (actual is not null)
            await _responsableData.Eliminar(actual);
        return RedirectToAction(nameof(Detalles), new { year = actual?.Year, idAreaOrganiz = actual?.idAreaOrganiz });
    }

    [HttpGet]
    public async Task<IActionResult> Locales()
    {
        return View(await _localData.Listar());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LocalNuevo(Local modelo)
    {
        if (string.IsNullOrWhiteSpace(modelo.NombreLocal))
            ModelState.AddModelError(nameof(modelo.NombreLocal), "El nombre del local es obligatorio.");

        if (ModelState.IsValid)
        {
            modelo.idLocal = 0;
            await _localData.Crear(modelo);
        }
        return RedirectToAction(nameof(Locales));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LocalEliminar(int idLocal)
    {
        var actual = await _localData.Obtener(idLocal);
        if (actual is not null)
            await _localData.Eliminar(actual);
        return RedirectToAction(nameof(Locales));
    }

    // ---------- Utilidades ----------

    private async Task CargarCatalogos(int year)
    {
        var areas = await _areaData.Listar();
        ViewBag.Year = year;
        ViewBag.Areas = areas.Where(a => a.Year == year).OrderBy(a => a.AreaOrganizacional).ToList();
        ViewBag.Empleados = await EmpleadosAsync();
    }

    private async Task<List<(int Id, string Nombre)>> EmpleadosAsync()
        => await _empleados.EmpleadosAsync();
}