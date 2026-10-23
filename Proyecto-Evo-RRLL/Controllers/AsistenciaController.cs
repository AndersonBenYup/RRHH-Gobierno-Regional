using Capa_Logica;
using Capa_Datos;
using Capa_Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Evo_RRLL.Models.ViewModels;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class AsistenciaController : Controller
{
    private readonly AsistenciaLogica _asistenciaData;
    private readonly MarcacionLogica _marcacionData;
    private readonly RegAsisDiarioLogica _regAsisData;
    private readonly HorarioTemporalLogica _horarioTemporalData;
    private readonly HorarioLogica _horarioData;
    private readonly EmpleadoLogica _empleadoData;
    private readonly PersonaLogica _personaData;
    private readonly TipoMarcacionLogica _tipoMarcacionData;
    private readonly DispositivoMarcacionLogica _dispositivoData;
    private readonly EstadoAsistenciaLogica _estadoAsistenciaData;
    private readonly SecuenciaService _secuencia;
    private readonly Services.EmpleadoServicio _empleados;

    public AsistenciaController(
        AsistenciaLogica asistenciaData,
        MarcacionLogica marcacionData,
        RegAsisDiarioLogica regAsisData,
        HorarioTemporalLogica horarioTemporalData,
        HorarioLogica horarioData,
        EmpleadoLogica empleadoData,
        PersonaLogica personaData,
        TipoMarcacionLogica tipoMarcacionData,
        DispositivoMarcacionLogica dispositivoData,
        EstadoAsistenciaLogica estadoAsistenciaData,
        SecuenciaService secuencia,
        Services.EmpleadoServicio empleados)
    {
        _asistenciaData = asistenciaData;
        _marcacionData = marcacionData;
        _regAsisData = regAsisData;
        _horarioTemporalData = horarioTemporalData;
        _horarioData = horarioData;
        _empleadoData = empleadoData;
        _personaData = personaData;
        _tipoMarcacionData = tipoMarcacionData;
        _dispositivoData = dispositivoData;
        _estadoAsistenciaData = estadoAsistenciaData;
        _secuencia = secuencia;
        _empleados = empleados;
    }

    [HttpGet]
    public async Task<IActionResult> Index(DateTime? desde, DateTime? hasta, string? q)
    {
        var registros = await _asistenciaData.Listar();
        var empleados = await _empleadoData.Listar();
        var personas = await _personaData.Listar();

        var desdeEf = desde ?? DateTime.Today.AddMonths(-1);
        var hastaEf = hasta ?? DateTime.Today.AddDays(1);

        var vm = new AsistenciaViewModel { Desde = desde, Hasta = hasta ?? DateTime.Today, Busqueda = q };

        foreach (var r in registros.Where(x => x.Fecha >= desdeEf && x.Fecha < hastaEf))
        {
            var empleado = empleados.FirstOrDefault(e => e.IdEmpleado == r.IdEmpleado);
            var persona = empleado?.IdPersona is null ? null : personas.FirstOrDefault(p => p.IdPersona == empleado.IdPersona);
            var nombre = persona is null
                ? $"(Empleado {r.IdEmpleado})"
                : $"{persona.Nombres} {persona.Apellido_Paterno} {persona.Apellido_Materno}".Trim();

            if (!string.IsNullOrWhiteSpace(q)
                && !nombre.Contains(q, StringComparison.OrdinalIgnoreCase)
                && !(persona?.NumDocID?.Contains(q, StringComparison.OrdinalIgnoreCase) == true))
                continue;

            vm.Registros.Add(new AsistenciaFila
            {
                Fecha = r.Fecha,
                IdEmpleado = r.IdEmpleado,
                Empleado = nombre,
                HorEntrada = r.Hor_Ent?.ToString("HH:mm:ss"),
                HorSalida = r.Hor_Sal?.ToString("HH:mm:ss"),
                AlmSalida = r.Alm_Sal?.ToString("HH:mm:ss"),
                AlmEntrada = r.Alm_Ent?.ToString("HH:mm:ss"),
                Estado = r.Estado
            });

            if (vm.Registros.Count >= 2000)
                break;
        }

        vm.Registros = vm.Registros.OrderByDescending(x => x.Fecha).ThenBy(x => x.IdEmpleado).ToList();
        return View(vm);
    }

    // ---------- Marcaciones ----------

    [HttpGet]
    public async Task<IActionResult> Marcaciones(DateTime? desde, DateTime? hasta, int? idEmpleado)
    {
        var registros = (await _marcacionData.Listar())
            .Where(m => idEmpleado is null || m.IdEmpleado == idEmpleado)
            .Where(m => !desde.HasValue || m.Fecha.Date >= desde.Value.Date)
            .Where(m => !hasta.HasValue || m.Fecha.Date <= hasta.Value.Date)
            .OrderByDescending(m => m.Fecha)
            .ThenBy(m => m.IdEmpleado)
            .Take(2000);

        var horarios = await _horarioData.Listar();

        ViewBag.Desde = desde;
        ViewBag.Hasta = hasta;
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Horarios = horarios;
        return View(registros.ToList());
    }

    [HttpGet]
    public async Task<IActionResult> MarcacionNuevo()
    {
        await CargarCatalogosMarcacion();
        return View("MarcacionForm", new Marcacion { Fecha = DateTime.Today });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarcacionNuevo(Marcacion modelo)
    {
        if (await _marcacionData.Obtener(modelo.Fecha, modelo.IdEmpleado) is not null)
            ModelState.AddModelError("", "Ya existe una marcación para este empleado en esa fecha.");

        if (ModelState.IsValid)
        {
            if (string.IsNullOrWhiteSpace(modelo.CodDispositivo))
                modelo.CodDispositivo = null;
            await _marcacionData.Crear(modelo);
            return RedirectToAction(nameof(Marcaciones), new { idEmpleado = modelo.IdEmpleado });
        }

        await CargarCatalogosMarcacion();
        return View("MarcacionForm", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> MarcacionEditar(DateTime fecha, int idEmpleado)
    {
        var registro = await _marcacionData.Obtener(fecha, idEmpleado);
        if (registro is null)
            return NotFound();

        await CargarCatalogosMarcacion();
        return View("MarcacionForm", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarcacionEditar(Marcacion modelo)
    {
        var actual = await _marcacionData.Obtener(modelo.Fecha, modelo.IdEmpleado);
        if (actual is null)
            return NotFound();

        actual.NroMarcacion = modelo.NroMarcacion;
        actual.TipoMarcacion = modelo.TipoMarcacion;
        actual.CodDispositivo = string.IsNullOrWhiteSpace(modelo.CodDispositivo) ? null : modelo.CodDispositivo;
        actual.idHorario = modelo.idHorario;
        await _marcacionData.Actualizar(actual);

        return RedirectToAction(nameof(Marcaciones), new { idEmpleado = modelo.IdEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarcacionEliminar(DateTime fecha, int idEmpleado)
    {
        var actual = await _marcacionData.Obtener(fecha, idEmpleado);
        if (actual is not null)
            await _marcacionData.Eliminar(actual);
        return RedirectToAction(nameof(Marcaciones), new { idEmpleado });
    }

    // ---------- Registro diario ----------

    [HttpGet]
    public async Task<IActionResult> RegDiario(DateTime? desde, DateTime? hasta, int? idEmpleado)
    {
        var registros = (await _regAsisData.Listar())
            .Where(m => idEmpleado is null || m.IdEmpleado == idEmpleado)
            .Where(m => !desde.HasValue || m.Fecha.Date >= desde.Value.Date)
            .Where(m => !hasta.HasValue || m.Fecha.Date <= hasta.Value.Date)
            .OrderByDescending(m => m.Fecha)
            .ThenBy(m => m.IdEmpleado)
            .Take(2000);

        ViewBag.Desde = desde;
        ViewBag.Hasta = hasta;
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        return View(registros.ToList());
    }

    [HttpGet]
    public async Task<IActionResult> RegDiarioNuevo()
    {
        await CargarCatalogosRegDiario();
        return View("RegDiarioForm", new RegAsisDiario { Fecha = DateTime.Today });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegDiarioNuevo(RegAsisDiario modelo)
    {
        if (await _regAsisData.Obtener(modelo.Fecha, modelo.IdEmpleado) is not null)
            ModelState.AddModelError("", "Ya existe un registro diario para este empleado en esa fecha.");

        if (ModelState.IsValid)
        {
            await _regAsisData.Crear(modelo);
            return RedirectToAction(nameof(RegDiario), new { idEmpleado = modelo.IdEmpleado });
        }

        await CargarCatalogosRegDiario();
        return View("RegDiarioForm", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> RegDiarioEditar(DateTime fecha, int idEmpleado)
    {
        var registro = await _regAsisData.Obtener(fecha, idEmpleado);
        if (registro is null)
            return NotFound();

        await CargarCatalogosRegDiario();
        return View("RegDiarioForm", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegDiarioEditar(RegAsisDiario modelo)
    {
        var actual = await _regAsisData.Obtener(modelo.Fecha, modelo.IdEmpleado);
        if (actual is null)
            return NotFound();

        actual.MinNormales = modelo.MinNormales;
        actual.SegundosTarde = modelo.SegundosTarde;
        actual.Estado = modelo.Estado;
        await _regAsisData.Actualizar(actual);

        return RedirectToAction(nameof(RegDiario), new { idEmpleado = modelo.IdEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegDiarioEliminar(DateTime fecha, int idEmpleado)
    {
        var actual = await _regAsisData.Obtener(fecha, idEmpleado);
        if (actual is not null)
            await _regAsisData.Eliminar(actual);
        return RedirectToAction(nameof(RegDiario), new { idEmpleado });
    }

    // ---------- Horarios temporales ----------

    [HttpGet]
    public async Task<IActionResult> HorariosTemporal(int? idEmpleado)
    {
        var registros = (await _horarioTemporalData.Listar())
            .Where(h => idEmpleado is null || h.idEmpleado == idEmpleado)
            .OrderByDescending(h => h.FechaInicio);

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Horarios = await _horarioData.Listar();
        return View(registros.ToList());
    }

    [HttpGet]
    public async Task<IActionResult> HorarioTemporalNuevo(int? idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Horarios = await _horarioData.Listar();
        return View("HorarioTemporalForm", new HorarioTemporal { idEmpleado = idEmpleado ?? 0, FechaRegistro = DateTime.Now });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> HorarioTemporalNuevo(HorarioTemporal modelo)
    {
        var guardo = await _secuencia.EjecutarConBloqueoAsync(
            async () =>
            {
                var lista = await _horarioTemporalData.Listar();
                return lista.Where(h => h.idEmpleado == modelo.idEmpleado).Select(h => h.N).DefaultIfEmpty(0).Max() + 1;
            },
            async id =>
            {
                modelo.N = id;
                await _horarioTemporalData.Crear(modelo);
                return true;
            });
        if (guardo)
            return RedirectToAction(nameof(HorariosTemporal), new { idEmpleado = modelo.idEmpleado });

        ViewBag.IdEmpleado = modelo.idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Horarios = await _horarioData.Listar();
        ModelState.AddModelError("", "No se pudo registrar el horario temporal: intente nuevamente.");
        return View("HorarioTemporalForm", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> HorarioTemporalEditar(int idEmpleado, int n)
    {
        var registro = await _horarioTemporalData.Obtener(idEmpleado, n);
        if (registro is null)
            return NotFound();

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Horarios = await _horarioData.Listar();
        return View("HorarioTemporalForm", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> HorarioTemporalEditar(HorarioTemporal modelo)
    {
        var actual = await _horarioTemporalData.Obtener(modelo.idEmpleado, modelo.N);
        if (actual is null)
            return NotFound();

        actual.FechaInicio = modelo.FechaInicio;
        actual.FechaFin = modelo.FechaFin;
        actual.HorarioAsignado = modelo.HorarioAsignado;
        actual.NroDocumento = modelo.NroDocumento;
        actual.Sisgedo = modelo.Sisgedo;
        actual.FechaRegistro = modelo.FechaRegistro;
        await _horarioTemporalData.Actualizar(actual);

        return RedirectToAction(nameof(HorariosTemporal), new { idEmpleado = modelo.idEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> HorarioTemporalEliminar(int idEmpleado, int n)
    {
        var actual = await _horarioTemporalData.Obtener(idEmpleado, n);
        if (actual is not null)
            await _horarioTemporalData.Eliminar(actual);
        return RedirectToAction(nameof(HorariosTemporal), new { idEmpleado });
    }

    // ---------- Utilidades ----------

    private async Task CargarCatalogosMarcacion()
    {
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Horarios = await _horarioData.Listar();
        ViewBag.TiposMarcacion = await _tipoMarcacionData.Listar();
        ViewBag.Dispositivos = await _dispositivoData.Listar();
    }

    private async Task CargarCatalogosRegDiario()
    {
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.EstadosAsistencia = await _estadoAsistenciaData.Listar();
    }

    private async Task<List<(int Id, string Nombre)>> EmpleadosAsync()
        => await _empleados.EmpleadosAsync();
}