using Capa_Logica;
using Capa_Datos;
using Capa_Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Evo_RRLL.Models.ViewModels;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class FamiliaSaludController : Controller
{
    private readonly EmpleadoLogica _empleadoData;
    private readonly PersonaLogica _personaData;
    private readonly FamiliarLogica _familiarData;
    private readonly RRHH_SaludLogica _saludData;
    private readonly RRHH_DinamicaFamiliarLogica _dinamicaData;
    private readonly RRHH_AspSocioLogica _aspSocioData;
    private readonly RRHH_FuncFamLogica _funcFamData;
    private readonly RRHH_AseguradoLogica _aseguradoData;

    private readonly RRHH_AcudeEnfermLogica _acudeEnfermData;
    private readonly RRHH_TipoFamiliaLogica _tipoFamiliaData;
    private readonly RRHH_RelacParejaLogica _relacParejaData;
    private readonly RRHH_RelacPHLogica _relacPHData;
    private readonly RRHH_RelacHermanoLogica _relacHermanoData;
    private readonly TipoFamiliarLogica _tipoFamiliarData;
    private readonly EstadoCivilLogica _estadoCivilData;
    private readonly TipoSangreLogica _tipoSangreData;
    private readonly DiscapacidadLogica _discapacidadData;
    private readonly FamiliarDetalleLogica _familiarDetalleData;
    private readonly VinculoAseguradoLogica _vinculoData;
    private readonly SecuenciaService _secuencia;
    private readonly Services.EmpleadoServicio _empleados;

    public FamiliaSaludController(
        EmpleadoLogica empleadoData,
        PersonaLogica personaData,
        FamiliarLogica familiarData,
        RRHH_SaludLogica saludData,
        RRHH_DinamicaFamiliarLogica dinamicaData,
        RRHH_AspSocioLogica aspSocioData,
        RRHH_FuncFamLogica funcFamData,
        RRHH_AseguradoLogica aseguradoData,
        RRHH_AcudeEnfermLogica acudeEnfermData,
        RRHH_TipoFamiliaLogica tipoFamiliaData,
        RRHH_RelacParejaLogica relacParejaData,
        RRHH_RelacPHLogica relacPHData,
        RRHH_RelacHermanoLogica relacHermanoData,
        TipoFamiliarLogica tipoFamiliarData,
        EstadoCivilLogica estadoCivilData,
        TipoSangreLogica tipoSangreData,
        DiscapacidadLogica discapacidadData,
        FamiliarDetalleLogica familiarDetalleData,
        VinculoAseguradoLogica vinculoData,
        SecuenciaService secuencia,
        Services.EmpleadoServicio empleados)
    {
        _empleadoData = empleadoData;
        _personaData = personaData;
        _familiarData = familiarData;
        _saludData = saludData;
        _dinamicaData = dinamicaData;
        _aspSocioData = aspSocioData;
        _funcFamData = funcFamData;
        _aseguradoData = aseguradoData;
        _acudeEnfermData = acudeEnfermData;
        _tipoFamiliaData = tipoFamiliaData;
        _relacParejaData = relacParejaData;
        _relacPHData = relacPHData;
        _relacHermanoData = relacHermanoData;
        _tipoFamiliarData = tipoFamiliarData;
        _estadoCivilData = estadoCivilData;
        _tipoSangreData = tipoSangreData;
        _discapacidadData = discapacidadData;
        _familiarDetalleData = familiarDetalleData;
        _vinculoData = vinculoData;
        _secuencia = secuencia;
        _empleados = empleados;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int idEmpleado)
    {
        if (await _empleadoData.Obtener(idEmpleado) is null)
            return NotFound();

        var salud = (await _saludData.Listar()).FirstOrDefault(s => s.IdEmpleado == idEmpleado);
        var dinamica = (await _dinamicaData.Listar()).FirstOrDefault(d => d.IdEmpleado == idEmpleado);
        var aspSocio = (await _aspSocioData.Listar()).FirstOrDefault(a => a.IdEmpleado == idEmpleado);
        var funcFam = (await _funcFamData.Listar()).FirstOrDefault(f => f.IdEmpleado == idEmpleado);
        var familiares = (await _familiarData.Listar()).Count(f => f.IdEmpleado == idEmpleado);
        var asegurados = (await _aseguradoData.Listar()).Count(a => a.idEmpleado == idEmpleado);

        return View(new FamiliaSaludViewModel
        {
            IdEmpleado = idEmpleado,
            NombreEmpleado = await NombreEmpleadoAsync(idEmpleado),
            TieneSalud = salud is not null,
            TieneDinamica = dinamica is not null,
            TieneAspSocio = aspSocio is not null,
            TieneFuncFam = funcFam is not null,
            Familiares = familiares,
            Asegurados = asegurados
        });
    }

    // ---------- Familiares (solo lectura) ----------

    [HttpGet]
    public async Task<IActionResult> Familiares(int idEmpleado)
    {
        var familiarRows = (await _familiarData.Listar()).Where(f => f.IdEmpleado == idEmpleado);
        var personas = await _personaData.Listar();
        var tipoFams = await _tipoFamiliarData.Listar();
        var estadoCiviles = await _estadoCivilData.Listar();
        var discapacidades = await _discapacidadData.Listar();
        var tipoSangres = await _tipoSangreData.Listar();
        var detalle = (await _familiarDetalleData.Listar()).Where(d => d.IdEmpleado == idEmpleado).ToList();

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.NombreEmpleado = await NombreEmpleadoAsync(idEmpleado);

        var filas = familiarRows.Select(f =>
        {
            var persona = personas.FirstOrDefault(p => p.IdPersona == f.IdPersona);
            return new FamiliarFila
            {
                IdEmpleado = f.IdEmpleado,
                IdPersona = f.IdPersona,
                Nombres = persona is null ? null : $"{persona.Nombres} {persona.Apellido_Paterno} {persona.Apellido_Materno}".Trim(),
                TipoFamiliar = f.IdTipoFam is null ? null : tipoFams.FirstOrDefault(t => t.IdTipoFam == f.IdTipoFam)?.DescripTipoFam,
                Sexo = persona?.Sexo,
                Ocupacion = persona?.Ocupacion,
                EstadoCivil = persona?.IdEstadoCivil is null ? null : estadoCiviles.FirstOrDefault(e => e.IdEstadoCivil == persona.IdEstadoCivil)?.DescripEstCivil,
                ViveCasa = f.ViveCasa == true ? "Sí" : f.ViveCasa == false ? "No" : null,
                LaboraInstitucion = detalle.FirstOrDefault(d => d.IdPersona == f.IdPersona)?.LaboraInstitucion switch { true => "Sí", false => "No", _ => null },
                Discapacidad = persona?.IdDiscapacidad is null ? null : discapacidades.FirstOrDefault(d => d.IdDiscapacidad == persona.IdDiscapacidad)?.Descrip_Discapacidad,
                TipoSangre = persona?.IdTipoSangre is null ? null : tipoSangres.FirstOrDefault(t => t.IdTipoSangre == persona.IdTipoSangre)?.DescripTS
            };
        }).ToList();

        return View(filas);
    }

    // ---------- Salud ----------

    [HttpGet]
    public async Task<IActionResult> Salud(int idEmpleado)
    {
        if (await _empleadoData.Obtener(idEmpleado) is null)
            return NotFound();

        var registro = (await _saludData.Listar()).FirstOrDefault(s => s.IdEmpleado == idEmpleado) ?? new RRHH_Salud();

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.NombreEmpleado = await NombreEmpleadoAsync(idEmpleado);
        ViewBag.Acudes = await _acudeEnfermData.Listar();
        return View(registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Salud(RRHH_Salud modelo)
    {
        var actual = (await _saludData.Listar()).FirstOrDefault(s => s.IdEmpleado == modelo.IdEmpleado);

        if (actual is null)
        {
            var guardo = await _secuencia.EjecutarTransaccionalAsync(async () =>
            {
                modelo.idSalud = 0;
                await _saludData.Crear(modelo);
            });
            if (!guardo)
            {
                TempData["MensajeError"] = "No se pudo guardar la ficha de salud: intente nuevamente.";
                return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado ?? 0 });
            }
        }
        else
        {
            actual.EnferCronica = modelo.EnferCronica;
            actual.EnferCronicaDescrip = modelo.EnferCronicaDescrip;
            actual.FamDiscapacidad = modelo.FamDiscapacidad;
            actual.FamDiscapDescrip = modelo.FamDiscapDescrip;
            actual.Alergico = modelo.Alergico;
            actual.AlergicoDescrip = modelo.AlergicoDescrip;
            actual.idAcudeEnferm = modelo.idAcudeEnferm;
            await _saludData.Actualizar(actual);
        }

        return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado ?? 0 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarSalud(int idEmpleado)
    {
        var actual = (await _saludData.Listar()).FirstOrDefault(s => s.IdEmpleado == idEmpleado);
        if (actual is not null)
            await _saludData.Eliminar(actual);
        return RedirectToAction(nameof(Index), new { idEmpleado });
    }

    // ---------- Ficha social (dinámica + aspecto + funcionalidad) ----------

    [HttpGet]
    public async Task<IActionResult> FichaSocial(int idEmpleado)
    {
        var vm = new FichaSocialViewModel { IdEmpleado = idEmpleado, NombreEmpleado = await NombreEmpleadoAsync(idEmpleado) };

        var dinamica = (await _dinamicaData.Listar()).FirstOrDefault(d => d.IdEmpleado == idEmpleado);
        var aspSocio = (await _aspSocioData.Listar()).FirstOrDefault(a => a.IdEmpleado == idEmpleado);
        var funcFam = (await _funcFamData.Listar()).FirstOrDefault(f => f.IdEmpleado == idEmpleado);

        if (dinamica is not null) vm.Dinamica = dinamica;
        if (aspSocio is not null) vm.AspSocio = aspSocio;
        if (funcFam is not null) vm.FuncFam = funcFam;

        ViewBag.TiposFamilia = await _tipoFamiliaData.Listar();
        ViewBag.RelacParejas = await _relacParejaData.Listar();
        ViewBag.RelacPHs = await _relacPHData.Listar();
        ViewBag.RelacHermanos = await _relacHermanoData.Listar();
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> FichaSocial(FichaSocialViewModel modelo)
    {
        var idEmpleado = modelo.IdEmpleado;

        var guardo = await _secuencia.EjecutarTransaccionalAsync(async () =>
        {
            var dinamicaActual = (await _dinamicaData.Listar()).FirstOrDefault(d => d.IdEmpleado == idEmpleado);
            if (dinamicaActual is null)
            {
                modelo.Dinamica.IdEmpleado = idEmpleado;
                modelo.Dinamica.idDinamica = 0;
                await _dinamicaData.Crear(modelo.Dinamica);
            }
            else
            {
                CopiarDinamica(dinamicaActual, modelo.Dinamica);
                await _dinamicaData.Actualizar(dinamicaActual);
            }

            var aspSocioActual = (await _aspSocioData.Listar()).FirstOrDefault(a => a.IdEmpleado == idEmpleado);
            if (aspSocioActual is null)
            {
                modelo.AspSocio.IdEmpleado = idEmpleado;
                modelo.AspSocio.idAspSocio = 0;
                await _aspSocioData.Crear(modelo.AspSocio);
            }
            else
            {
                CopiarAspSocio(aspSocioActual, modelo.AspSocio);
                await _aspSocioData.Actualizar(aspSocioActual);
            }

            var funcFamActual = (await _funcFamData.Listar()).FirstOrDefault(f => f.IdEmpleado == idEmpleado);
            if (funcFamActual is null)
            {
                modelo.FuncFam.IdEmpleado = idEmpleado;
                await _funcFamData.Crear(modelo.FuncFam);
            }
            else
            {
                CopiarFuncFam(funcFamActual, modelo.FuncFam);
                await _funcFamData.Actualizar(funcFamActual);
            }
        });

        if (!guardo)
            TempData["MensajeError"] = "No se pudo guardar la ficha social: intente nuevamente.";

        return RedirectToAction(nameof(Index), new { idEmpleado });
    }

    // ---------- Asegurados ----------

    [HttpGet]
    public async Task<IActionResult> Asegurados(int idEmpleado)
    {
        var asegurados = (await _aseguradoData.Listar()).Where(a => a.idEmpleado == idEmpleado);
        var personas = await _personaData.Listar();
        var estadoCiviles = await _estadoCivilData.Listar();
        var vinculos = await _vinculoData.Listar();

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.NombreEmpleado = await NombreEmpleadoAsync(idEmpleado);

        var filas = asegurados.Select(a =>
        {
            var persona = a.idPersona is null ? null : personas.FirstOrDefault(p => p.IdPersona == a.idPersona);
            return new AseguradoFila
            {
                NroBeneficiario = a.NroBeneficiario,
                Nombre = persona is null ? null : $"{persona.Nombres} {persona.Apellido_Paterno} {persona.Apellido_Materno}".Trim(),
                EstadoCivil = persona?.IdEstadoCivil is null ? null : estadoCiviles.FirstOrDefault(e => e.IdEstadoCivil == persona.IdEstadoCivil)?.DescripEstCivil,
                VinculoFamiliar = vinculos.FirstOrDefault(v => v.IdVinculoAsegurado == a.IdVinculoAsegurado)?.Descripcion,
                Sexo = persona?.Sexo,
                FechaInicio = a.FechaInicioVinculo,
                FechaFin = a.FechaFinVinculo
            };
        }).ToList();

        return View(filas);
    }

    [HttpGet]
    public async Task<IActionResult> AseguradoCrear(int idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.NombreEmpleado = await NombreEmpleadoAsync(idEmpleado);
        ViewBag.EsEdicion = false;
        ViewBag.Vinculos = await _vinculoData.Listar();
        return View("AseguradoForm", new RRHH_Asegurado { idEmpleado = idEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AseguradoCrear(RRHH_Asegurado modelo)
    {
        if (modelo.NroBeneficiario == 0)
            modelo.NroBeneficiario = (await _aseguradoData.Listar())
                .Where(a => a.idEmpleado == modelo.idEmpleado).Select(a => a.NroBeneficiario).DefaultIfEmpty(0).Max() + 1;
        await _aseguradoData.Crear(modelo);
        return RedirectToAction(nameof(Asegurados), new { idEmpleado = modelo.idEmpleado });
    }

    [HttpGet]
    public async Task<IActionResult> AseguradoEditar(int idEmpleado, int nro)
    {
        var registro = await _aseguradoData.Obtener(idEmpleado, nro);
        if (registro is null)
            return NotFound();

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.NombreEmpleado = await NombreEmpleadoAsync(idEmpleado);
        ViewBag.EsEdicion = true;
        ViewBag.Vinculos = await _vinculoData.Listar();
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

        return RedirectToAction(nameof(Asegurados), new { idEmpleado = modelo.idEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AseguradoEliminar(int idEmpleado, int nro)
    {
        var actual = await _aseguradoData.Obtener(idEmpleado, nro);
        if (actual is not null)
            await _aseguradoData.Eliminar(actual);
        return RedirectToAction(nameof(Asegurados), new { idEmpleado });
    }

    // ---------- Utilidades ----------

    private async Task<string?> NombreEmpleadoAsync(int idEmpleado)
        => await _empleados.NombreEmpleadoAsync(idEmpleado);

    private static void CopiarDinamica(RRHH_DinamicaFamiliar destino, RRHH_DinamicaFamiliar origen)
    {
        destino.idTipoFam = origen.idTipoFam;
        destino.idRelaPareja = origen.idRelaPareja;
        destino.idRelaPH = origen.idRelaPH;
        destino.idRelacHermano = origen.idRelacHermano;
        destino.Observacion = origen.Observacion;
    }

    private static void CopiarAspSocio(RRHH_AspSocio destino, RRHH_AspSocio origen)
    {
        destino.PrincipProblemas = origen.PrincipProblemas;
        destino.PropMejoraInstitu = origen.PropMejoraInstitu;
        destino.hobby = origen.hobby;
        destino.ActRecreaInstitu = origen.ActRecreaInstitu;
    }

    private static void CopiarFuncFam(RRHH_FuncFam destino, RRHH_FuncFam origen)
    {
        destino.Com1 = origen.Com1;
        destino.Com2 = origen.Com2;
        destino.Afec1 = origen.Afec1;
        destino.Afec2 = origen.Afec2;
        destino.Decis1 = origen.Decis1;
        destino.Decis2 = origen.Decis2;
        destino.Soc1 = origen.Soc1;
        destino.Soc2 = origen.Soc2;
        destino.Soc3 = origen.Soc3;
    }
}