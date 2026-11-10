using Capa_Logica.Exportacion;
using Capa_Logica;
using Capa_Entidades.Contratos;
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
    private readonly ISecuenciaService _secuencia;
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
        ISecuenciaService secuencia,
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
    public async Task<IActionResult> Index(DateTime? desde, DateTime? hasta, string? q,
        [FromServices] RRHH_FeriadoLogica feriados)
    {
        var vm = new AsistenciaViewModel { Desde = desde, Hasta = hasta ?? DateTime.Today, Busqueda = q };
        vm.Registros = await ConstruirFilasAsync(desde, hasta, q, limite: 2000);
        var rango = Rango(desde, hasta);
        vm.Feriados = Capa_Logica.Reglas.FeriadosEnRango.Fechas(await feriados.Listar(), rango.Desde, rango.HastaExclusivo);
        return View(vm);
    }

    /// <summary>SC-003: exporta a CSV el registro de asistencia con los mismos filtros de la pantalla (sin límite de filas).</summary>
    [HttpGet]
    public async Task<IActionResult> Exportar(DateTime? desde, DateTime? hasta, string? q, [FromServices] ExportadorTablas exportador)
    {
        var filas = new List<string?[]>
        {
            new[] { "Fecha", "IdEmpleado", "Empleado", "Entrada", "Salida almuerzo", "Entrada almuerzo", "Salida", "Estado" }
        };
        foreach (var r in await ConstruirFilasAsync(desde, hasta, q, limite: null))
            filas.Add(new string?[]
            {
                r.Fecha.ToString("dd/MM/yyyy"), r.IdEmpleado.ToString(), r.Empleado,
                r.HorEntrada, r.AlmSalida, r.AlmEntrada, r.HorSalida,
                r.Estado switch { true => "Presente", false => "Ausente", _ => "" }
            });

        var (desdeEf, hastaEf) = Rango(desde, hasta);
        var archivo = exportador.Exportar(filas, $"asistencia_{desdeEf:yyyyMMdd}_{hastaEf.AddDays(-1):yyyyMMdd}");
        return File(archivo.Contenido, archivo.TipoContenido, archivo.NombreArchivo);
    }

    /// <summary>Por defecto, el último mes; <c>hasta</c> se incluye completo.</summary>
    private static (DateTime Desde, DateTime HastaExclusivo) Rango(DateTime? desde, DateTime? hasta)
        => ((desde ?? DateTime.Today.AddMonths(-1)).Date, (hasta ?? DateTime.Today).Date.AddDays(1));

    /// <summary>
    /// Filas del registro de asistencia. El rango de fechas se filtra en la BD (antes se traía toda la tabla);
    /// el filtro por nombre o DNI se aplica después sobre ese rango.
    /// </summary>
    private async Task<List<AsistenciaFila>> ConstruirFilasAsync(DateTime? desde, DateTime? hasta, string? q, int? limite)
    {
        var (desdeEf, hastaEf) = Rango(desde, hasta);
        var registros = await _asistenciaData.ListarEntreFechas(desdeEf, hastaEf);
        var empleados = (await _empleadoData.Listar()).ToDictionary(e => e.IdEmpleado);
        var personas = (await _personaData.Listar()).ToDictionary(p => p.IdPersona);

        var filas = new List<AsistenciaFila>();
        foreach (var r in registros.OrderByDescending(x => x.Fecha).ThenBy(x => x.IdEmpleado))
        {
            var empleado = empleados.GetValueOrDefault(r.IdEmpleado);
            var persona = empleado?.IdPersona is null ? null : personas.GetValueOrDefault(empleado.IdPersona.Value);
            var nombre = persona is null
                ? $"(Empleado {r.IdEmpleado})"
                : $"{persona.Nombres} {persona.Apellido_Paterno} {persona.Apellido_Materno}".Trim();

            if (!string.IsNullOrWhiteSpace(q)
                && !nombre.Contains(q, StringComparison.OrdinalIgnoreCase)
                && !(persona?.NumDocID?.Contains(q, StringComparison.OrdinalIgnoreCase) == true))
                continue;

            filas.Add(new AsistenciaFila
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

            if (limite is not null && filas.Count >= limite)
                break;
        }
        return filas;
    }

    // ---------- Marcaciones ----------

    [HttpGet]
    public async Task<IActionResult> Marcaciones(DateTime? desde, DateTime? hasta, int? idEmpleado)
    {
        var registros = await _marcacionData.ListarRecientes(idEmpleado, desde, hasta);

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
        if (!ModelState.IsValid)
            return await FormularioAsync("MarcacionForm", modelo, modelo.IdEmpleado);

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
        var registros = await _regAsisData.ListarRecientes(idEmpleado, desde, hasta);

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
        if (!ModelState.IsValid)
            return await FormularioAsync("RegDiarioForm", modelo, modelo.IdEmpleado);

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
    public async Task<IActionResult> HorariosTemporal(int? idEmpleado, DateOnly? desde, DateOnly? hasta)
    {
        if (desde.HasValue && hasta.HasValue && desde > hasta)
            ModelState.AddModelError("", "La fecha desde debe ser anterior o igual a la fecha hasta.");

        var registros = new List<HorarioTemporal>();
        if (ModelState.IsValid)
        {
            registros = (await _horarioTemporalData.Listar())
                .Where(h => idEmpleado is null || h.idEmpleado == idEmpleado)
                .Where(h => !desde.HasValue || (h.FechaFin ?? h.FechaInicio) >= desde)
                .Where(h => !hasta.HasValue || h.FechaInicio <= hasta)
                .OrderByDescending(h => h.FechaInicio)
                .ToList();
        }

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Desde = desde?.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        ViewBag.Hasta = hasta?.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Horarios = await _horarioData.Listar();
        return View(registros);
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
        if (!ModelState.IsValid)
            return await FormularioAsync("HorarioTemporalForm", modelo, modelo.idEmpleado);

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
        if (!ModelState.IsValid)
            return await FormularioAsync("HorarioTemporalForm", modelo, modelo.idEmpleado);

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

    // SC-008: vuelve a mostrar el formulario con los errores de validación.
    private async Task<IActionResult> FormularioAsync(string vista, object modelo, int? idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        await CargarCatalogosMarcacion();
        ViewBag.EstadosAsistencia = await _estadoAsistenciaData.Listar();
        return View(vista, modelo);
    }
}
