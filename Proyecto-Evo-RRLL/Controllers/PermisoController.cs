using Capa_Logica;
using Capa_Entidades.Contratos;
using Capa_Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Evo_RRLL.Models.ViewModels;
using Proyecto_Evo_RRLL.Utilidades;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class PermisoController : Controller
{
    private readonly PermisoLogica _permisoData;
    private readonly Motivo_PermLogica _motivoData;
    private readonly EmpleadoLogica _empleadoData;
    private readonly PersonaLogica _personaData;
    private readonly ISecuenciaService _secuencia;
    private readonly Services.EmpleadoServicio _empleados;

    public PermisoController(
        PermisoLogica permisoData,
        Motivo_PermLogica motivoData,
        EmpleadoLogica empleadoData,
        PersonaLogica personaData,
        ISecuenciaService secuencia,
        Services.EmpleadoServicio empleados)
    {
        _permisoData = permisoData;
        _motivoData = motivoData;
        _empleadoData = empleadoData;
        _personaData = personaData;
        _secuencia = secuencia;
        _empleados = empleados;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? idEmpleado, int pagina = 1, int? tamano = null)
    {
        // SC-005: el filtro y la página se resuelven en SQL Server (OFFSET/FETCH); antes se traía toda la tabla.
        var tamanoPagina = Capa_Logica.Paginacion.VentanaPaginas.NormalizarTamano(tamano, PermisoLogica.TamanoPagina);
        var resultado = await _permisoData.ListarPaginado(idEmpleado, pagina, tamanoPagina);

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.NombreEmpleado = idEmpleado is null ? null : await NombreEmpleadoAsync(idEmpleado.Value);
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Motivos = await _motivoData.Listar();
        ViewBag.Paginador = new PaginadorModelo(resultado.NumeroPagina, resultado.TotalPaginas, resultado.Total,
            nameof(Index), new Dictionary<string, string?> { ["idEmpleado"] = idEmpleado?.ToString() }, tamanoPagina, "permisos");
        return View(resultado.Registros);
    }

    [HttpGet]
    public async Task<IActionResult> Nuevo(int? idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Motivos = await _motivoData.Listar();
        return View("Form", new Permiso { IdEmpleado = idEmpleado ?? 0 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Nuevo(Permiso modelo)
    {
        if (ModelState.IsValid)
            ModelState.AgregarErrores(await _permisoData.Validar(modelo));
        if (!ModelState.IsValid)
            return await FormularioAsync(modelo);

        var guardo = await _secuencia.EjecutarConBloqueoAsync(
            () => _permisoData.SiguienteNumero(modelo.IdEmpleado), // SC-005: MAX + 1 en la BD
            async id =>
            {
                modelo.NPermiso = id;
                await _permisoData.Crear(modelo);
                return true;
            });
        if (!guardo)
            ModelState.AddModelError("", "No se pudo registrar el permiso: intente nuevamente.");
        else
        {
            TempData["MensajeExito"] = "Permiso registrado.";
            return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado });
        }

        return await FormularioAsync(modelo);
    }

    private async Task<IActionResult> FormularioAsync(Permiso modelo)
    {
        ViewBag.IdEmpleado = modelo.IdEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Motivos = await _motivoData.Listar();
        return View("Form", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int idEmpleado, int npermiso)
    {
        var registro = await _permisoData.Obtener(idEmpleado, npermiso);
        if (registro is null)
            return NotFound();

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Motivos = await _motivoData.Listar();
        return View("Form", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(Permiso modelo)
    {
        var actual = await _permisoData.Obtener(modelo.IdEmpleado, modelo.NPermiso);
        if (actual is null)
            return NotFound();

        if (ModelState.IsValid)
            ModelState.AgregarErrores(await _permisoData.Validar(modelo));
        if (!ModelState.IsValid)
            return await FormularioAsync(modelo);

        actual.FechaInicio = modelo.FechaInicio;
        actual.FechaFin = modelo.FechaFin;
        actual.Hora_Sal = modelo.Hora_Sal;
        actual.Hora_Ret = modelo.Hora_Ret;
        actual.Dia = modelo.Dia;
        actual.Retorno = modelo.Retorno;
        actual.idMotivo = modelo.idMotivo;
        actual.Lugar = modelo.Lugar;
        actual.Referencia = modelo.Referencia;
        actual.Autorizacion = modelo.Autorizacion;
        actual.Obs = modelo.Obs;
        actual.AutorizacionRRHH = modelo.AutorizacionRRHH;
        actual.Autorizado = modelo.Autorizado;
        await _permisoData.Actualizar(actual);

        TempData["MensajeExito"] = "Permiso actualizado.";
        return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int idEmpleado, int npermiso)
    {
        var actual = await _permisoData.Obtener(idEmpleado, npermiso);
        if (actual is not null)
            await _permisoData.Eliminar(actual);
        return RedirectToAction(nameof(Index), new { idEmpleado });
    }

    [HttpGet]
    public async Task<IActionResult> Teletrabajo(int? idEmpleado)
    {
        ViewBag.Empleados = await EmpleadosAsync();
        return View(new PeriodoTeletrabajo { IdEmpleado = idEmpleado ?? 0 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Teletrabajo(
        [Bind("IdEmpleado,FechaInicio,FechaFin,Parcial")] PeriodoTeletrabajo modelo,
        [FromServices] IRepositorio<PeriodoTeletrabajo> periodos,
        [FromServices] ILogger<PermisoController> logger)
    {
        var empleados = await EmpleadosAsync();
        if (ModelState.IsValid && !empleados.Any(e => e.Id == modelo.IdEmpleado))
            ModelState.AddModelError(nameof(modelo.IdEmpleado), "Seleccione un empleado válido.");

        if (ModelState.IsValid)
        {
            try
            {
                await periodos.Crear(modelo);
                TempData["MensajeExito"] = "Periodo de teletrabajo registrado.";
                return RedirectToAction(nameof(Teletrabajo), new { idEmpleado = modelo.IdEmpleado });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "No se pudo registrar el periodo de teletrabajo.");
                ModelState.AddModelError("", "No se pudo guardar el periodo. Verifique si ya está registrado.");
            }
        }

        ViewBag.Empleados = empleados;
        return View(modelo);
    }

    [HttpGet]
    public async Task<IActionResult> ReporteTeletrabajo(
        int? idEmpleado, DateOnly? desde, DateOnly? hasta, bool? parcial,
        [FromServices] IRepositorio<PeriodoTeletrabajo> periodos)
    {
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Parcial = parcial;
        ViewBag.Desde = desde?.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        ViewBag.Hasta = hasta?.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

        if (desde.HasValue && hasta.HasValue && desde > hasta)
            ModelState.AddModelError("", "La fecha de inicio no puede ser posterior a la fecha de fin.");

        if (!ModelState.IsValid || desde is not DateOnly inicio || hasta is not DateOnly fin)
            return View(Array.Empty<PeriodoTeletrabajo>());

        var registros = (await periodos.Listar())
            .Where(p => (!idEmpleado.HasValue || p.IdEmpleado == idEmpleado)
                && (!parcial.HasValue || p.Parcial == parcial)
                && p.FechaInicio <= fin && p.FechaFin >= inicio)
            .OrderBy(p => p.IdEmpleado)
            .ThenBy(p => p.FechaInicio)
            .ToList();
        ViewBag.ReporteConsultado = true;
        if (idEmpleado.HasValue)
            ViewBag.TotalDias = Capa_Logica.Reglas.DiasTeletrabajo.Contar(registros, inicio, fin);
        return View(registros);
    }

    [HttpGet]
    public async Task<IActionResult> ReporteAusencias(DateOnly? desde, DateOnly? hasta, int[]? motivos)
    {
        var catalogo = (await _motivoData.Listar()).Where(m => m.idMotivo is not (125 or 126))
            .OrderBy(m => m.Descrip_Motivo).ToList();
        var seleccion = (motivos ?? []).Intersect(catalogo.Select(m => m.idMotivo)).ToHashSet();
        ViewBag.Catalogo = catalogo;
        ViewBag.Seleccion = seleccion;
        ViewBag.Desde = desde?.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        ViewBag.Hasta = hasta?.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

        if (desde.HasValue && hasta.HasValue && desde > hasta)
            ModelState.AddModelError("", "La fecha desde no puede ser posterior a la fecha hasta.");

        if (!ModelState.IsValid || desde is not DateOnly inicio || hasta is not DateOnly fin || seleccion.Count == 0)
            return View(Array.Empty<Permiso>());

        ViewBag.Empleados = await EmpleadosAsync();
        var resultado = (await _permisoData.Listar())
            .Where(p => p.idMotivo is int motivo && seleccion.Contains(motivo) && p.FechaInicio is DateTime fecha
                && DateOnly.FromDateTime(fecha) <= fin && DateOnly.FromDateTime(p.FechaFin ?? fecha) >= inicio)
            .OrderBy(p => p.FechaInicio).ThenBy(p => p.IdEmpleado).ToList();
        return View(resultado);
    }

    // ---------- Utilidades ----------

    private async Task<List<(int Id, string Nombre)>> EmpleadosAsync()
        => await _empleados.EmpleadosAsync();

    private async Task<string?> NombreEmpleadoAsync(int idEmpleado)
        => await _empleados.NombreEmpleadoAsync(idEmpleado);
}
