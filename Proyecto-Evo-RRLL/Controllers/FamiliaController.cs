using Capa_Logica;
using Capa_Datos;
using Capa_Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Evo_RRLL.Models.ViewModels;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class FamiliaController : Controller
{
    private readonly FamiliarLogica _familiarData;
    private readonly RRHH_DinamicaFamiliarLogica _dinamicaData;
    private readonly RRHH_FuncFamLogica _funcFamData;
    private readonly EmpleadoLogica _empleadoData;
    private readonly PersonaLogica _personaData;
    private readonly TipoFamiliarLogica _tipoFamiliarData;
    private readonly RRHH_TipoFamiliaLogica _tipoFamiliaData;
    private readonly RRHH_RelacParejaLogica _relacParejaData;
    private readonly RRHH_RelacPHLogica _relacPHData;
    private readonly RRHH_RelacHermanoLogica _relacHermanoData;
    private readonly EstadoCivilLogica _estadoCivilData;
    private readonly DiscapacidadLogica _discapacidadData;
    private readonly TipoSangreLogica _tipoSangreData;
    private readonly FamiliarDetalleLogica _familiarDetalleData;
    private readonly SecuenciaService _secuencia;
    private readonly Services.EmpleadoServicio _empleados;

    public FamiliaController(
        FamiliarLogica familiarData,
        RRHH_DinamicaFamiliarLogica dinamicaData,
        RRHH_FuncFamLogica funcFamData,
        EmpleadoLogica empleadoData,
        PersonaLogica personaData,
        TipoFamiliarLogica tipoFamiliarData,
        RRHH_TipoFamiliaLogica tipoFamiliaData,
        RRHH_RelacParejaLogica relacParejaData,
        RRHH_RelacPHLogica relacPHData,
        RRHH_RelacHermanoLogica relacHermanoData,
        EstadoCivilLogica estadoCivilData,
        DiscapacidadLogica discapacidadData,
        TipoSangreLogica tipoSangreData,
        FamiliarDetalleLogica familiarDetalleData,
        SecuenciaService secuencia,
        Services.EmpleadoServicio empleados)
    {
        _familiarData = familiarData;
        _dinamicaData = dinamicaData;
        _funcFamData = funcFamData;
        _empleadoData = empleadoData;
        _personaData = personaData;
        _tipoFamiliarData = tipoFamiliarData;
        _tipoFamiliaData = tipoFamiliaData;
        _relacParejaData = relacParejaData;
        _relacPHData = relacPHData;
        _relacHermanoData = relacHermanoData;
        _estadoCivilData = estadoCivilData;
        _discapacidadData = discapacidadData;
        _tipoSangreData = tipoSangreData;
        _familiarDetalleData = familiarDetalleData;
        _secuencia = secuencia;
        _empleados = empleados;
    }

    // ---------- Familiares ----------

    [HttpGet]
    public async Task<IActionResult> Index(int? idEmpleado)
    {
        var familiares = (await _familiarData.Listar())
            .Where(f => idEmpleado is null || f.IdEmpleado == idEmpleado)
            .OrderBy(f => f.IdEmpleado)
            .ThenBy(f => f.IdPersona)
            .ToList();
        var personas = (await _personaData.Listar()).ToDictionary(p => p.IdPersona);
        var fichas = familiares
            .Select(f => FamiliarFicha.Desde(f, personas.GetValueOrDefault(f.IdPersona), null))
            .ToList();

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Personas = await PersonasAsync();
        ViewBag.TiposFamiliar = await _tipoFamiliarData.Listar();
        ViewBag.EstadosCiviles = await _estadoCivilData.Listar();
        ViewBag.Discapacidades = await _discapacidadData.Listar();
        ViewBag.TiposSangre = await _tipoSangreData.Listar();
        return View(fichas);
    }

    [HttpGet]
    public async Task<IActionResult> FamiliarNuevo(int? idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Personas = await PersonasAsync();
        ViewBag.TiposFamiliar = await _tipoFamiliarData.Listar();
        ViewBag.EstadosCiviles = await _estadoCivilData.Listar();
        ViewBag.Discapacidades = await _discapacidadData.Listar();
        ViewBag.TiposSangre = await _tipoSangreData.Listar();
        return View("FamiliarForm", new FamiliarFicha { IdEmpleado = idEmpleado ?? 0 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> FamiliarNuevo(FamiliarFicha modelo)
    {
        if (await _familiarData.Obtener(modelo.IdEmpleado, modelo.IdPersona) is not null)
            ModelState.AddModelError("", "Ese familiar ya está registrado para el empleado.");

        var persona = await _personaData.Obtener(modelo.IdPersona);
        if (persona is null)
            ModelState.AddModelError(nameof(modelo.IdPersona), "Seleccione una persona válida.");

        if (ModelState.IsValid)
        {
            var familiar = new Familiar { IdEmpleado = modelo.IdEmpleado, IdPersona = modelo.IdPersona };
            modelo.AplicarA(familiar);
            modelo.AplicarA(persona!);
            await _familiarData.Crear(familiar);
            await _personaData.Actualizar(persona!);
            TempData["MensajeExito"] = "Familiar registrado.";
            return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado });
        }

        ViewBag.IdEmpleado = modelo.IdEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Personas = await PersonasAsync();
        ViewBag.TiposFamiliar = await _tipoFamiliarData.Listar();
        ViewBag.EstadosCiviles = await _estadoCivilData.Listar();
        ViewBag.Discapacidades = await _discapacidadData.Listar();
        ViewBag.TiposSangre = await _tipoSangreData.Listar();
        return View("FamiliarForm", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> FamiliarEditar(int idEmpleado, int idPersona)
    {
        var familiar = await _familiarData.Obtener(idEmpleado, idPersona);
        if (familiar is null)
            return NotFound();
        var registro = FamiliarFicha.Desde(familiar, await _personaData.Obtener(idPersona),
            (await _familiarDetalleData.Obtener(idEmpleado, idPersona))?.LaboraInstitucion);

        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Personas = await PersonasAsync();
        ViewBag.TiposFamiliar = await _tipoFamiliarData.Listar();
        ViewBag.EstadosCiviles = await _estadoCivilData.Listar();
        ViewBag.Discapacidades = await _discapacidadData.Listar();
        ViewBag.TiposSangre = await _tipoSangreData.Listar();
        return View("FamiliarForm", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> FamiliarEditar(FamiliarFicha modelo)
    {
        var actual = await _familiarData.Obtener(modelo.IdEmpleado, modelo.IdPersona);
        if (actual is null)
            return NotFound();

        modelo.AplicarA(actual);
        await _familiarData.Actualizar(actual);
        if (await _personaData.Obtener(modelo.IdPersona) is { } persona)
        {
            modelo.AplicarA(persona);
            await _personaData.Actualizar(persona);
        }
        TempData["MensajeExito"] = "Familiar actualizado.";
        return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> FamiliarEliminar(int idEmpleado, int idPersona)
    {
        var actual = await _familiarData.Obtener(idEmpleado, idPersona);
        if (actual is not null)
        {
            await _familiarData.Eliminar(actual);
            TempData["MensajeExito"] = "Familiar eliminado.";
        }
        return RedirectToAction(nameof(Index), new { idEmpleado });
    }

    // ---------- Dinámica familiar ----------

    [HttpGet]
    public async Task<IActionResult> Dinamica(int? idEmpleado)
    {
        var registros = (await _dinamicaData.Listar())
            .Where(d => idEmpleado is null || d.IdEmpleado == idEmpleado)
            .OrderByDescending(d => d.idDinamica);

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposFamilia = await _tipoFamiliaData.Listar();
        ViewBag.RelacPareja = await _relacParejaData.Listar();
        ViewBag.RelacPH = await _relacPHData.Listar();
        ViewBag.RelacHermano = await _relacHermanoData.Listar();
        return View(registros.ToList());
    }

    [HttpGet]
    public async Task<IActionResult> DinamicaNuevo(int? idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposFamilia = await _tipoFamiliaData.Listar();
        ViewBag.RelacPareja = await _relacParejaData.Listar();
        ViewBag.RelacPH = await _relacPHData.Listar();
        ViewBag.RelacHermano = await _relacHermanoData.Listar();
        return View("DinamicaForm", new RRHH_DinamicaFamiliar { IdEmpleado = idEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DinamicaNuevo(RRHH_DinamicaFamiliar modelo)
    {
        if (ModelState.IsValid)
        {
            var guardo = await _secuencia.EjecutarTransaccionalAsync(async () =>
            {
                modelo.idDinamica = 0;
                await _dinamicaData.Crear(modelo);
            });
            if (guardo)
            {
                TempData["MensajeExito"] = "Dinámica familiar guardada.";
                return RedirectToAction(nameof(Dinamica), new { idEmpleado = modelo.IdEmpleado });
            }
            ModelState.AddModelError("", "No se pudo guardar la dinámica familiar: intente nuevamente.");
        }

        ViewBag.IdEmpleado = modelo.IdEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposFamilia = await _tipoFamiliaData.Listar();
        ViewBag.RelacPareja = await _relacParejaData.Listar();
        ViewBag.RelacPH = await _relacPHData.Listar();
        ViewBag.RelacHermano = await _relacHermanoData.Listar();
        return View("DinamicaForm", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> DinamicaEditar(int id)
    {
        var registro = await _dinamicaData.Obtener(id);
        if (registro is null)
            return NotFound();

        ViewBag.IdEmpleado = registro.IdEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposFamilia = await _tipoFamiliaData.Listar();
        ViewBag.RelacPareja = await _relacParejaData.Listar();
        ViewBag.RelacPH = await _relacPHData.Listar();
        ViewBag.RelacHermano = await _relacHermanoData.Listar();
        return View("DinamicaForm", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DinamicaEditar(RRHH_DinamicaFamiliar modelo)
    {
        var actual = await _dinamicaData.Obtener(modelo.idDinamica);
        if (actual is null)
            return NotFound();

        actual.IdEmpleado = modelo.IdEmpleado;
        actual.idTipoFam = modelo.idTipoFam;
        actual.idRelaPareja = modelo.idRelaPareja;
        actual.idRelaPH = modelo.idRelaPH;
        actual.idRelacHermano = modelo.idRelacHermano;
        actual.Observacion = modelo.Observacion;
        await _dinamicaData.Actualizar(actual);
        TempData["MensajeExito"] = "Dinámica familiar actualizada.";
        return RedirectToAction(nameof(Dinamica), new { idEmpleado = actual.IdEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DinamicaEliminar(int id)
    {
        var actual = await _dinamicaData.Obtener(id);
        if (actual is not null)
        {
            await _dinamicaData.Eliminar(actual);
            TempData["MensajeExito"] = "Dinámica familiar eliminada.";
        }
        return RedirectToAction(nameof(Dinamica), new { idEmpleado = actual?.IdEmpleado });
    }

    // ---------- Funcionalidad familiar ----------

    [HttpGet]
    public async Task<IActionResult> FuncFam(int? idEmpleado)
    {
        var registros = (await _funcFamData.Listar())
            .Where(f => idEmpleado is null || f.IdEmpleado == idEmpleado)
            .OrderBy(f => f.IdEmpleado);

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        return View(registros.ToList());
    }

    [HttpGet]
    public async Task<IActionResult> FuncFamNuevo(int? idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.EsNuevo = true;
        return View("FuncFamForm", new RRHH_FuncFam { IdEmpleado = idEmpleado ?? 0 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> FuncFamNuevo(RRHH_FuncFam modelo)
    {
        if (await _funcFamData.Obtener(modelo.IdEmpleado) is not null)
            ModelState.AddModelError("", "El empleado ya tiene una evaluación de funcionalidad familiar: edítela.");

        if (ModelState.IsValid)
        {
            await _funcFamData.Crear(modelo);
            TempData["MensajeExito"] = "Funcionalidad familiar guardada.";
            return RedirectToAction(nameof(FuncFam), new { idEmpleado = modelo.IdEmpleado });
        }

        ViewBag.EsNuevo = true;
        ViewBag.IdEmpleado = modelo.IdEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        return View("FuncFamForm", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> FuncFamEditar(int id)
    {
        var registro = await _funcFamData.Obtener(id);
        if (registro is null)
            return NotFound();

        ViewBag.IdEmpleado = registro.IdEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        return View("FuncFamForm", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> FuncFamEditar(RRHH_FuncFam modelo)
    {
        var actual = await _funcFamData.Obtener(modelo.IdEmpleado);
        if (actual is null)
            return NotFound();

        actual.Com1 = modelo.Com1;
        actual.Com2 = modelo.Com2;
        actual.Afec1 = modelo.Afec1;
        actual.Afec2 = modelo.Afec2;
        actual.Decis1 = modelo.Decis1;
        actual.Decis2 = modelo.Decis2;
        actual.Soc1 = modelo.Soc1;
        actual.Soc2 = modelo.Soc2;
        actual.Soc3 = modelo.Soc3;
        await _funcFamData.Actualizar(actual);
        TempData["MensajeExito"] = "Funcionalidad familiar actualizada.";
        return RedirectToAction(nameof(FuncFam), new { idEmpleado = actual.IdEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> FuncFamEliminar(int id)
    {
        var actual = await _funcFamData.Obtener(id);
        if (actual is not null)
        {
            await _funcFamData.Eliminar(actual);
            TempData["MensajeExito"] = "Funcionalidad familiar eliminada.";
        }
        return RedirectToAction(nameof(FuncFam), new { idEmpleado = actual?.IdEmpleado });
    }

    // ---------- Utilidades ----------

    private async Task<List<(int Id, string Nombre)>> EmpleadosAsync()
        => await _empleados.EmpleadosAsync();

    private async Task<List<(int Id, string Nombre)>> PersonasAsync()
    {
        var personas = await _personaData.Listar();
        return personas
            .Select(p => (p.IdPersona, $"{p.Nombres} {p.Apellido_Paterno} {p.Apellido_Materno}".Trim()))
            .OrderBy(x => x.Item2)
            .Select(x => (x.Item1, x.Item2))
            .ToList();
    }
}