using Capa_Logica;
using Capa_Datos;
using Capa_Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Evo_RRLL.Models.ViewModels;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class HistorialLaboralController : Controller
{
    private readonly PeriodoLaboralLogica _periodoData;
    private readonly ExpLaboralLogica _expData;
    private readonly RotacionLogica _rotacionData;
    private readonly ResolucionLogica _resolucionData;
    private readonly TipoResolucionLogica _tipoResolucionData;
    private readonly TipoInstitucionLogica _tipoInstitucionData;
    private readonly EstructOrganizLogica _estructOrganizData;
    private readonly EmpleadoLogica _empleadoData;
    private readonly PersonaLogica _personaData;
    private readonly SecuenciaService _secuencia;
    private readonly Services.EmpleadoServicio _empleados;

    public HistorialLaboralController(
        PeriodoLaboralLogica periodoData,
        ExpLaboralLogica expData,
        RotacionLogica rotacionData,
        ResolucionLogica resolucionData,
        TipoResolucionLogica tipoResolucionData,
        TipoInstitucionLogica tipoInstitucionData,
        EstructOrganizLogica estructuraOrganizData,
        EmpleadoLogica empleadoData,
        PersonaLogica personaData,
        SecuenciaService secuencia,
        Services.EmpleadoServicio empleados)
    {
        _periodoData = periodoData;
        _expData = expData;
        _rotacionData = rotacionData;
        _resolucionData = resolucionData;
        _tipoResolucionData = tipoResolucionData;
        _tipoInstitucionData = tipoInstitucionData;
        _estructOrganizData = estructuraOrganizData;
        _empleadoData = empleadoData;
        _personaData = personaData;
        _secuencia = secuencia;
        _empleados = empleados;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? idEmpleado)
    {
        var periodos = (await _periodoData.Listar())
            .Where(p => idEmpleado is null || p.IdEmpleado == idEmpleado)
            .OrderByDescending(p => p.FechaIngreso);
        var exp = (await _expData.Listar())
            .Where(e => idEmpleado is null || e.IdEmpleado == idEmpleado)
            .OrderBy(e => e.IdExpLab);
        var rotaciones = (await _rotacionData.Listar())
            .Where(r => idEmpleado is null || r.idEmpleado == idEmpleado)
            .OrderByDescending(r => r.FechaMemo);
        var resoluciones = (await _resolucionData.Listar())
            .Where(r => idEmpleado is null || r.idEmpleado == idEmpleado)
            .OrderByDescending(r => r.FechaResolucion);

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.NombreEmpleado = idEmpleado is null ? null : await NombreEmpleadoAsync(idEmpleado.Value);
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposResolucion = await _tipoResolucionData.Listar();
        ViewBag.TiposInstitucion = await _tipoInstitucionData.Listar();
        ViewBag.Areas = await _estructOrganizData.Listar();
        return View(new HistorialLaboralModelo
        {
            Periodos = periodos.ToList(),
            Experiencias = exp.ToList(),
            Rotaciones = rotaciones.ToList(),
            Resoluciones = resoluciones.ToList()
        });
    }

    // ---------- Períodos laborales ----------

    [HttpGet]
    public async Task<IActionResult> PeriodoNuevo(int? idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        return View("PeriodoForm", new PeriodoLaboral { IdEmpleado = idEmpleado ?? 0 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PeriodoNuevo(PeriodoLaboral modelo)
    {
        var guardo = await _secuencia.EjecutarConBloqueoAsync(
            async () =>
            {
                var lista = await _periodoData.Listar();
                return lista.Where(p => p.IdEmpleado == modelo.IdEmpleado).Select(p => p.NroPeriodo).DefaultIfEmpty(0).Max() + 1;
            },
            async id =>
            {
                modelo.NroPeriodo = id;
                await _periodoData.Crear(modelo);
                return true;
            });
        if (guardo)
            return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado });

        ViewBag.IdEmpleado = modelo.IdEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ModelState.AddModelError("", "No se pudo registrar el período: intente nuevamente.");
        return View("PeriodoForm", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> PeriodoEditar(int idEmpleado, int nroPeriodo)
    {
        var registro = await _periodoData.Obtener(idEmpleado, nroPeriodo);
        if (registro is null)
            return NotFound();

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        return View("PeriodoForm", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PeriodoEditar(PeriodoLaboral modelo)
    {
        var actual = await _periodoData.Obtener(modelo.IdEmpleado, modelo.NroPeriodo);
        if (actual is null)
            return NotFound();

        actual.NumResoIngreInstitu = modelo.NumResoIngreInstitu;
        actual.FechaSalida = modelo.FechaSalida;
        actual.FechaResoIngreInstitu = modelo.FechaResoIngreInstitu;
        actual.FechaIngreso = modelo.FechaIngreso;
        actual.Judicial = modelo.Judicial;
        actual.Obs = modelo.Obs;
        await _periodoData.Actualizar(actual);

        return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PeriodoEliminar(int idEmpleado, int nroPeriodo)
    {
        var actual = await _periodoData.Obtener(idEmpleado, nroPeriodo);
        if (actual is not null)
            await _periodoData.Eliminar(actual);
        return RedirectToAction(nameof(Index), new { idEmpleado });
    }

    // ---------- Experiencia laboral ----------

    [HttpGet]
    public async Task<IActionResult> ExpNuevo(int? idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposInstitucion = await _tipoInstitucionData.Listar();
        return View("ExpForm", new ExpLaboral { IdEmpleado = idEmpleado ?? 0 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ExpNuevo(ExpLaboral modelo)
    {
        var guardo = await _secuencia.EjecutarConBloqueoAsync(
            async () =>
            {
                var lista = await _expData.Listar();
                return lista.Where(e => e.IdEmpleado == modelo.IdEmpleado).Select(e => e.IdExpLab).DefaultIfEmpty(0).Max() + 1;
            },
            async id =>
            {
                modelo.IdExpLab = id;
                await _expData.Crear(modelo);
                return true;
            });
        if (guardo)
            return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado });

        ViewBag.IdEmpleado = modelo.IdEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposInstitucion = await _tipoInstitucionData.Listar();
        ModelState.AddModelError("", "No se pudo registrar la experiencia: intente nuevamente.");
        return View("ExpForm", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> ExpEditar(int idEmpleado, int idExpLab)
    {
        var registro = await _expData.Obtener(idEmpleado, idExpLab);
        if (registro is null)
            return NotFound();

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposInstitucion = await _tipoInstitucionData.Listar();
        return View("ExpForm", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ExpEditar(ExpLaboral modelo)
    {
        var actual = await _expData.Obtener(modelo.IdEmpleado, modelo.IdExpLab);
        if (actual is null)
            return NotFound();

        actual.NomInstitucion = modelo.NomInstitucion;
        actual.Cargo = modelo.Cargo;
        actual.FechaIngreso = modelo.FechaIngreso;
        actual.FechaEgreso = modelo.FechaEgreso;
        actual.UnidadOrganica = modelo.UnidadOrganica;
        actual.IdTipoInstitucion = modelo.IdTipoInstitucion;
        await _expData.Actualizar(actual);

        return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ExpEliminar(int idEmpleado, int idExpLab)
    {
        var actual = await _expData.Obtener(idEmpleado, idExpLab);
        if (actual is not null)
            await _expData.Eliminar(actual);
        return RedirectToAction(nameof(Index), new { idEmpleado });
    }

    // ---------- Rotaciones ----------

    [HttpGet]
    public async Task<IActionResult> RotacionNuevo(int? idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Areas = await _estructOrganizData.Listar();
        return View("RotacionForm", new Rotacion { idEmpleado = idEmpleado ?? 0 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RotacionNuevo(Rotacion modelo)
    {
        var guardo = await _secuencia.EjecutarConBloqueoAsync(
            async () =>
            {
                var lista = await _rotacionData.Listar();
                return lista.Where(r => r.idEmpleado == modelo.idEmpleado).Select(r => r.idRotacion).DefaultIfEmpty(0).Max() + 1;
            },
            async id =>
            {
                modelo.idRotacion = id;
                await _rotacionData.Crear(modelo);
                return true;
            });
        if (guardo)
            return RedirectToAction(nameof(Index), new { idEmpleado = modelo.idEmpleado });

        ViewBag.IdEmpleado = modelo.idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Areas = await _estructOrganizData.Listar();
        ModelState.AddModelError("", "No se pudo registrar la rotación: intente nuevamente.");
        return View("RotacionForm", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> RotacionEditar(int idEmpleado, int idRotacion)
    {
        var registro = await _rotacionData.Obtener(idEmpleado, idRotacion);
        if (registro is null)
            return NotFound();

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Areas = await _estructOrganizData.Listar();
        return View("RotacionForm", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RotacionEditar(Rotacion modelo)
    {
        var actual = await _rotacionData.Obtener(modelo.idEmpleado, modelo.idRotacion);
        if (actual is null)
            return NotFound();

        actual.FechaMemo = modelo.FechaMemo;
        actual.NroMemo = modelo.NroMemo;
        actual.idAreaOrganiz = modelo.idAreaOrganiz;
        actual.Year = modelo.Year;
        await _rotacionData.Actualizar(actual);

        return RedirectToAction(nameof(Index), new { idEmpleado = modelo.idEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RotacionEliminar(int idEmpleado, int idRotacion)
    {
        var actual = await _rotacionData.Obtener(idEmpleado, idRotacion);
        if (actual is not null)
            await _rotacionData.Eliminar(actual);
        return RedirectToAction(nameof(Index), new { idEmpleado });
    }

    // ---------- Resoluciones ----------

    [HttpGet]
    public async Task<IActionResult> ResolucionNuevo(int? idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposResolucion = await _tipoResolucionData.Listar();
        return View("ResolucionForm", new Resolucion { idEmpleado = idEmpleado ?? 0 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResolucionNuevo(Resolucion modelo)
    {
        var guardo = await _secuencia.EjecutarConBloqueoAsync(
            async () =>
            {
                var lista = await _resolucionData.Listar();
                return lista.Where(r => r.idEmpleado == modelo.idEmpleado).Select(r => r.idResolucion).DefaultIfEmpty(0).Max() + 1;
            },
            async id =>
            {
                modelo.idResolucion = id;
                await _resolucionData.Crear(modelo);
                return true;
            });
        if (guardo)
            return RedirectToAction(nameof(Index), new { idEmpleado = modelo.idEmpleado });

        ViewBag.IdEmpleado = modelo.idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposResolucion = await _tipoResolucionData.Listar();
        ModelState.AddModelError("", "No se pudo registrar la resolución: intente nuevamente.");
        return View("ResolucionForm", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> ResolucionEditar(int idEmpleado, int idResolucion)
    {
        var registro = await _resolucionData.Obtener(idEmpleado, idResolucion);
        if (registro is null)
            return NotFound();

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposResolucion = await _tipoResolucionData.Listar();
        return View("ResolucionForm", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResolucionEditar(Resolucion modelo)
    {
        var actual = await _resolucionData.Obtener(modelo.idEmpleado, modelo.idResolucion);
        if (actual is null)
            return NotFound();

        actual.idTipoResolucion = modelo.idTipoResolucion;
        actual.FechaResolucion = modelo.FechaResolucion;
        actual.NumeroResolucion = modelo.NumeroResolucion;
        await _resolucionData.Actualizar(actual);

        return RedirectToAction(nameof(Index), new { idEmpleado = modelo.idEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResolucionEliminar(int idEmpleado, int idResolucion)
    {
        var actual = await _resolucionData.Obtener(idEmpleado, idResolucion);
        if (actual is not null)
            await _resolucionData.Eliminar(actual);
        return RedirectToAction(nameof(Index), new { idEmpleado });
    }

    // ---------- Utilidades ----------

    private async Task<List<(int Id, string Nombre)>> EmpleadosAsync()
        => await _empleados.EmpleadosAsync();

    private async Task<string?> NombreEmpleadoAsync(int idEmpleado)
        => await _empleados.NombreEmpleadoAsync(idEmpleado);
}