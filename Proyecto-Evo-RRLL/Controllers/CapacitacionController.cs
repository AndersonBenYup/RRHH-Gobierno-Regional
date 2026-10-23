using Capa_Logica;
using Capa_Datos;
using Capa_Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class CapacitacionController : Controller
{
    private readonly CapacitacionLogica _capacitacionData;
    private readonly EstudiosRealizadoLogica _estudiosData;
    private readonly TitulosEmpleadoLogica _titulosData;
    private readonly TipoEstudioLogica _tipoEstudioData;
    private readonly EstadoEstudioLogica _estadoEstudioData;
    private readonly EmpleadoLogica _empleadoData;
    private readonly PersonaLogica _personaData;
    private readonly SecuenciaService _secuencia;
    private readonly Services.EmpleadoServicio _empleados;

    public CapacitacionController(
        CapacitacionLogica capacitacionData,
        EstudiosRealizadoLogica estudiosData,
        TitulosEmpleadoLogica titulosData,
        TipoEstudioLogica tipoEstudioData,
        EstadoEstudioLogica estadoEstudioData,
        EmpleadoLogica empleadoData,
        PersonaLogica personaData,
        SecuenciaService secuencia,
        Services.EmpleadoServicio empleados)
    {
        _capacitacionData = capacitacionData;
        _estudiosData = estudiosData;
        _titulosData = titulosData;
        _tipoEstudioData = tipoEstudioData;
        _estadoEstudioData = estadoEstudioData;
        _empleadoData = empleadoData;
        _personaData = personaData;
        _secuencia = secuencia;
        _empleados = empleados;
    }

    // ---------- Cursos de capacitación ----------

    [HttpGet]
    public async Task<IActionResult> Index(int? idEmpleado, string? q)
    {
        var cursos = (await _capacitacionData.Listar())
            .Where(c => idEmpleado is null || c.IdEmpleado == idEmpleado)
            .OrderByDescending(c => c.FechaInicio);

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.NombreEmpleado = idEmpleado is null ? null : await NombreEmpleadoAsync(idEmpleado.Value);
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Busqueda = q;
        return View(cursos.ToList());
    }

    [HttpGet]
    public async Task<IActionResult> Nuevo(int? idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        return View("Form", new Capacitacion { IdEmpleado = idEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Nuevo(Capacitacion modelo)
    {
        modelo.IdCapacitacion = 0;
        await _capacitacionData.Crear(modelo);
        return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado });
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var registro = await _capacitacionData.Obtener(id);
        if (registro is null)
            return NotFound();

        ViewBag.IdEmpleado = registro.IdEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        return View("Form", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, Capacitacion modelo)
    {
        if (id != modelo.IdCapacitacion)
            return NotFound();

        var actual = await _capacitacionData.Obtener(id);
        if (actual is null)
            return NotFound();

        actual.NombreCurso = modelo.NombreCurso;
        actual.FechaInicio = modelo.FechaInicio;
        actual.FechaFin = modelo.FechaFin;
        actual.NumHoras = modelo.NumHoras;
        actual.InstitucionOrganizadora = modelo.InstitucionOrganizadora;
        actual.IdEmpleado = modelo.IdEmpleado;
        await _capacitacionData.Actualizar(actual);

        return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int id, int? idEmpleado)
    {
        var actual = await _capacitacionData.Obtener(id);
        if (actual is not null)
            await _capacitacionData.Eliminar(actual);
        return RedirectToAction(nameof(Index), new { idEmpleado });
    }

    // ---------- Estudios realizados ----------

    [HttpGet]
    public async Task<IActionResult> Estudios(int? idEmpleado)
    {
        var estudios = (await _estudiosData.Listar())
            .Where(e => idEmpleado is null || e.IdEmpleado == idEmpleado)
            .OrderBy(e => e.IdTipoEstudios).ThenBy(e => e.Correlativo);

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.NombreEmpleado = idEmpleado is null ? null : await NombreEmpleadoAsync(idEmpleado.Value);
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposEstudio = await _tipoEstudioData.Listar();
        ViewBag.EstadosEstudio = await _estadoEstudioData.Listar();
        return View(estudios.ToList());
    }

    [HttpGet]
    public async Task<IActionResult> EstudioNuevo(int? idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposEstudio = await _tipoEstudioData.Listar();
        ViewBag.EstadosEstudio = await _estadoEstudioData.Listar();
        return View("EstudioForm", new EstudiosRealizado { IdEmpleado = idEmpleado ?? 0 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EstudioNuevo(EstudiosRealizado modelo)
    {
        var guardo = await _secuencia.EjecutarConBloqueoAsync(
            async () =>
            {
                var lista = await _estudiosData.Listar();
                var maxCorr = lista
                    .Where(e => e.IdTipoEstudios == modelo.IdTipoEstudios && e.IdEmpleado == modelo.IdEmpleado)
                    .Select(e => e.Correlativo)
                    .DefaultIfEmpty(0)
                    .Max();
                return maxCorr + 1;
            },
            async id =>
            {
                modelo.Correlativo = id;
                await _estudiosData.Crear(modelo);
                return true;
            });
        if (guardo)
            return RedirectToAction(nameof(Estudios), new { idEmpleado = modelo.IdEmpleado });

        ViewBag.IdEmpleado = modelo.IdEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposEstudio = await _tipoEstudioData.Listar();
        ViewBag.EstadosEstudio = await _estadoEstudioData.Listar();
        ModelState.AddModelError("", "No se pudo registrar el estudio: intente nuevamente.");
        return View("EstudioForm", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> EstudioEditar(int idTipo, int idEmpleado, int correlativo)
    {
        var registro = await _estudiosData.Obtener(idTipo, idEmpleado, correlativo);
        if (registro is null)
            return NotFound();

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposEstudio = await _tipoEstudioData.Listar();
        ViewBag.EstadosEstudio = await _estadoEstudioData.Listar();
        return View("EstudioForm", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EstudioEditar(EstudiosRealizado modelo)
    {
        var actual = await _estudiosData.Obtener(modelo.IdTipoEstudios, modelo.IdEmpleado, modelo.Correlativo);
        if (actual is null)
            return NotFound();

        actual.CentroEstudios = modelo.CentroEstudios;
        actual.FechaInicio = modelo.FechaInicio;
        actual.Especialidad = modelo.Especialidad;
        actual.FechaTermino = modelo.FechaTermino;
        actual.IdEstadoEstudio = modelo.IdEstadoEstudio;
        await _estudiosData.Actualizar(actual);

        return RedirectToAction(nameof(Estudios), new { idEmpleado = modelo.IdEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EstudioEliminar(int idTipo, int idEmpleado, int correlativo)
    {
        var actual = await _estudiosData.Obtener(idTipo, idEmpleado, correlativo);
        if (actual is not null)
            await _estudiosData.Eliminar(actual);
        return RedirectToAction(nameof(Estudios), new { idEmpleado });
    }

    // ---------- Títulos ----------

    [HttpGet]
    public async Task<IActionResult> Titulos(int? idEmpleado)
    {
        var titulos = (await _titulosData.Listar())
            .Where(t => idEmpleado is null || t.IdEmpleado == idEmpleado)
            .OrderBy(t => t.NumColegiatura);

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.NombreEmpleado = idEmpleado is null ? null : await NombreEmpleadoAsync(idEmpleado.Value);
        ViewBag.Empleados = await EmpleadosAsync();
        return View(titulos.ToList());
    }

    [HttpGet]
    public async Task<IActionResult> TituloNuevo(int? idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        return View("TituloForm", new TitulosEmpleado { IdEmpleado = idEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TituloNuevo(TitulosEmpleado modelo)
    {
        modelo.IdTitulo = 0;
        await _titulosData.Crear(modelo);
        return RedirectToAction(nameof(Titulos), new { idEmpleado = modelo.IdEmpleado });
    }

    [HttpGet]
    public async Task<IActionResult> TituloEditar(int id)
    {
        var registro = await _titulosData.Obtener(id);
        if (registro is null)
            return NotFound();

        ViewBag.IdEmpleado = registro.IdEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        return View("TituloForm", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TituloEditar(int id, TitulosEmpleado modelo)
    {
        var actual = await _titulosData.Obtener(id);
        if (actual is null)
            return NotFound();

        actual.NumColegiatura = modelo.NumColegiatura;
        actual.DenominacionGrado = modelo.DenominacionGrado;
        actual.Institucion = modelo.Institucion;
        actual.Fecha = modelo.Fecha;
        actual.IdEmpleado = modelo.IdEmpleado;
        await _titulosData.Actualizar(actual);

        return RedirectToAction(nameof(Titulos), new { idEmpleado = modelo.IdEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TituloEliminar(int id, int? idEmpleado)
    {
        var actual = await _titulosData.Obtener(id);
        if (actual is not null)
            await _titulosData.Eliminar(actual);
        return RedirectToAction(nameof(Titulos), new { idEmpleado });
    }

    // ---------- Utilidades ----------

    private async Task<List<(int Id, string Nombre)>> EmpleadosAsync()
        => await _empleados.EmpleadosAsync();

    private async Task<string?> NombreEmpleadoAsync(int idEmpleado)
        => await _empleados.NombreEmpleadoAsync(idEmpleado);
}