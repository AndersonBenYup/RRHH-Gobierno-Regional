using Capa_Logica;
using Capa_Datos;
using Capa_Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class SaludController : Controller
{
    private readonly RRHH_SaludLogica _saludData;
    private readonly RRHH_AseguradoLogica _aseguradoData;
    private readonly RRHH_AcudeEnfermLogica _acudeEnfermData;
    private readonly EmpleadoLogica _empleadoData;
    private readonly PersonaLogica _personaData;
    private readonly EstadoCivilLogica _estadoCivilData;
    private readonly MotivoBajaLogica _motivoBajaData;
    private readonly VinculoAseguradoLogica _vinculoData;
    private readonly SecuenciaService _secuencia;
    private readonly Services.EmpleadoServicio _empleados;

    public SaludController(
        RRHH_SaludLogica saludData,
        RRHH_AseguradoLogica aseguradoData,
        RRHH_AcudeEnfermLogica acudeEnfermData,
        EmpleadoLogica empleadoData,
        PersonaLogica personaData,
        EstadoCivilLogica estadoCivilData,
        MotivoBajaLogica motivoBajaData,
        VinculoAseguradoLogica vinculoData,
        SecuenciaService secuencia,
        Services.EmpleadoServicio empleados)
    {
        _saludData = saludData;
        _aseguradoData = aseguradoData;
        _acudeEnfermData = acudeEnfermData;
        _empleadoData = empleadoData;
        _personaData = personaData;
        _estadoCivilData = estadoCivilData;
        _motivoBajaData = motivoBajaData;
        _vinculoData = vinculoData;
        _secuencia = secuencia;
        _empleados = empleados;
    }

    // ---------- Ficha de salud ----------

    [HttpGet]
    public async Task<IActionResult> Index(int? idEmpleado)
    {
        var fichas = (await _saludData.Listar())
            .Where(f => idEmpleado is null || f.IdEmpleado == idEmpleado)
            .OrderByDescending(f => f.idSalud);

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.AcudeEnferm = await _acudeEnfermData.Listar();
        return View(fichas.ToList());
    }

    [HttpGet]
    public async Task<IActionResult> SaludNuevo(int? idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.AcudeEnferm = await _acudeEnfermData.Listar();
        return View("SaludForm", new RRHH_Salud { IdEmpleado = idEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaludNuevo(RRHH_Salud modelo)
    {
        if (ModelState.IsValid)
        {
            var guardo = await _secuencia.EjecutarTransaccionalAsync(async () =>
            {
                modelo.idSalud = 0;
                await _saludData.Crear(modelo);
            });
            if (guardo)
            {
                TempData["MensajeExito"] = "Ficha de salud guardada.";
                return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado });
            }
            ModelState.AddModelError("", "No se pudo guardar la ficha de salud: intente nuevamente.");
        }

        ViewBag.IdEmpleado = modelo.IdEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.AcudeEnferm = await _acudeEnfermData.Listar();
        return View("SaludForm", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> SaludEditar(int id)
    {
        var ficha = await _saludData.Obtener(id);
        if (ficha is null)
            return NotFound();

        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.AcudeEnferm = await _acudeEnfermData.Listar();
        return View("SaludForm", ficha);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaludEditar(RRHH_Salud modelo)
    {
        var actual = await _saludData.Obtener(modelo.idSalud);
        if (actual is null)
            return NotFound();

        actual.IdEmpleado = modelo.IdEmpleado;
        actual.EnferCronica = modelo.EnferCronica;
        actual.EnferCronicaDescrip = modelo.EnferCronicaDescrip;
        actual.FamDiscapacidad = modelo.FamDiscapacidad;
        actual.FamDiscapDescrip = modelo.FamDiscapDescrip;
        actual.Alergico = modelo.Alergico;
        actual.AlergicoDescrip = modelo.AlergicoDescrip;
        actual.idAcudeEnferm = modelo.idAcudeEnferm;
        await _saludData.Actualizar(actual);
        TempData["MensajeExito"] = "Ficha de salud actualizada.";
        return RedirectToAction(nameof(Index), new { idEmpleado = actual.IdEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaludEliminar(int id)
    {
        var actual = await _saludData.Obtener(id);
        if (actual is not null)
        {
            await _saludData.Eliminar(actual);
            TempData["MensajeExito"] = "Ficha de salud eliminada.";
        }
        return RedirectToAction(nameof(Index), new { idEmpleado = actual?.IdEmpleado });
    }

    // ---------- Asegurados (beneficiarios) ----------

    [HttpGet]
    public async Task<IActionResult> Asegurados(int? idEmpleado)
    {
        var registros = (await _aseguradoData.Listar())
            .Where(a => idEmpleado is null || a.idEmpleado == idEmpleado)
            .OrderBy(a => a.idEmpleado)
            .ThenBy(a => a.NroBeneficiario);

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Personas = await PersonasAsync();
        ViewBag.Vinculos = await _vinculoData.Listar();
        ViewBag.SexoPorPersona = (await _personaData.Listar()).ToDictionary(p => p.IdPersona, p => p.Sexo);
        return View(registros.ToList());
    }

    [HttpGet]
    public async Task<IActionResult> AseguradoNuevo(int? idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Personas = await PersonasAsync();
        ViewBag.Vinculos = await _vinculoData.Listar();
        ViewBag.MotivosBaja = await _motivoBajaData.Listar();
        return View("AseguradoForm", new RRHH_Asegurado { idEmpleado = idEmpleado ?? 0 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AseguradoNuevo(RRHH_Asegurado modelo)
    {
        var guardo = await _secuencia.EjecutarConBloqueoAsync(
            async () =>
            {
                var lista = await _aseguradoData.Listar();
                return lista.Where(a => a.idEmpleado == modelo.idEmpleado).Select(a => a.NroBeneficiario).DefaultIfEmpty(0).Max() + 1;
            },
            async id =>
            {
                modelo.NroBeneficiario = id;
                await _aseguradoData.Crear(modelo);
                return true;
            });
        if (!guardo)
            ModelState.AddModelError("", "No se pudo agregar el beneficiario: intente nuevamente.");
        else
        {
            TempData["MensajeExito"] = "Beneficiario agregado.";
            return RedirectToAction(nameof(Asegurados), new { idEmpleado = modelo.idEmpleado });
        }

        ViewBag.IdEmpleado = modelo.idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Personas = await PersonasAsync();
        ViewBag.Vinculos = await _vinculoData.Listar();
        ViewBag.MotivosBaja = await _motivoBajaData.Listar();
        return View("AseguradoForm", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> AseguradoEditar(int idEmpleado, int nroBeneficiario)
    {
        var registro = await _aseguradoData.Obtener(idEmpleado, nroBeneficiario);
        if (registro is null)
            return NotFound();

        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Personas = await PersonasAsync();
        ViewBag.Vinculos = await _vinculoData.Listar();
        ViewBag.MotivosBaja = await _motivoBajaData.Listar();
        return View("AseguradoForm", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AseguradoEditar(RRHH_Asegurado modelo)
    {
        var actual = await _aseguradoData.Obtener(modelo.idEmpleado, modelo.NroBeneficiario);
        if (actual is null)
            return NotFound();

        actual.idPersona = modelo.idPersona;
        actual.IdVinculoAsegurado = modelo.IdVinculoAsegurado;
        actual.NroPartida = modelo.NroPartida;
        actual.NroResolMayorIndiscap = modelo.NroResolMayorIndiscap;
        actual.FechaInicioVinculo = modelo.FechaInicioVinculo;
        actual.idMotivoBaja = modelo.idMotivoBaja;
        actual.NroPartidaDef = modelo.NroPartidaDef;
        actual.OtrosMotivos = modelo.OtrosMotivos;
        actual.FechaFinVinculo = modelo.FechaFinVinculo;
        actual.Reevaluar = modelo.Reevaluar;
        await _aseguradoData.Actualizar(actual);
        TempData["MensajeExito"] = "Beneficiario actualizado.";
        return RedirectToAction(nameof(Asegurados), new { idEmpleado = modelo.idEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AseguradoEliminar(int idEmpleado, int nroBeneficiario)
    {
        var actual = await _aseguradoData.Obtener(idEmpleado, nroBeneficiario);
        if (actual is not null)
        {
            await _aseguradoData.Eliminar(actual);
            TempData["MensajeExito"] = "Beneficiario eliminado.";
        }
        return RedirectToAction(nameof(Asegurados), new { idEmpleado });
    }

    // ---------- Catálogo: a quién acudir ----------

    [HttpGet]
    public async Task<IActionResult> AcudeEnferm()
    {
        ViewBag.Empleados = await EmpleadosAsync();
        return View((await _acudeEnfermData.Listar()).OrderBy(a => a.idAcudeEnferm).ToList());
    }

    [HttpGet]
    public IActionResult AcudeEnfermNuevo()
    {
        return View("AcudeEnfermForm", new RRHH_AcudeEnferm());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AcudeEnfermNuevo(RRHH_AcudeEnferm modelo)
    {
        if (ModelState.IsValid)
        {
            var guardo = await _secuencia.EjecutarTransaccionalAsync(async () =>
            {
                modelo.idAcudeEnferm = 0;
                await _acudeEnfermData.Crear(modelo);
            });
            if (guardo)
            {
                TempData["MensajeExito"] = "Opción agregada.";
                return RedirectToAction(nameof(AcudeEnferm));
            }
            ModelState.AddModelError("", "No se pudo agregar la opción: intente nuevamente.");
        }
        return View("AcudeEnfermForm", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> AcudeEnfermEditar(int id)
    {
        var registro = await _acudeEnfermData.Obtener(id);
        if (registro is null)
            return NotFound();
        return View("AcudeEnfermForm", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AcudeEnfermEditar(RRHH_AcudeEnferm modelo)
    {
        var actual = await _acudeEnfermData.Obtener(modelo.idAcudeEnferm);
        if (actual is null)
            return NotFound();

        actual.AcudeEnfermDescrip = modelo.AcudeEnfermDescrip;
        await _acudeEnfermData.Actualizar(actual);
        TempData["MensajeExito"] = "Opción actualizada.";
        return RedirectToAction(nameof(AcudeEnferm));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AcudeEnfermEliminar(int id)
    {
        var actual = await _acudeEnfermData.Obtener(id);
        if (actual is not null)
        {
            var enUso = (await _saludData.Listar()).Any(s => s.idAcudeEnferm == id);
            if (enUso)
                TempData["MensajeError"] = "No se puede eliminar: la opción está en uso por fichas de salud.";
            else
            {
                await _acudeEnfermData.Eliminar(actual);
                TempData["MensajeExito"] = "Opción eliminada.";
            }
        }
        return RedirectToAction(nameof(AcudeEnferm));
    }

    // ---------- Utilidades ----------

    private async Task<List<(int Id, string Nombre)>> EmpleadosAsync()
        => await _empleados.EmpleadosAsync();

    private async Task<List<(int Id, string Nombre)>> PersonasAsync()
    {
        var personas = await _personaData.Listar();
        return personas.Select(p => (p.IdPersona, $"{p.Nombres} {p.Apellido_Paterno} {p.Apellido_Materno}".Trim()))
            .OrderBy(x => x.Item2)
            .Select(x => (x.Item1, x.Item2))
            .ToList();
    }
}