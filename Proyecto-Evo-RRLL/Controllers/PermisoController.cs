using Capa_Logica;
using Capa_Datos;
using Capa_Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class PermisoController : Controller
{
    private readonly PermisoLogica _permisoData;
    private readonly Motivo_PermLogica _motivoData;
    private readonly EmpleadoLogica _empleadoData;
    private readonly PersonaLogica _personaData;
    private readonly SecuenciaService _secuencia;
    private readonly Services.EmpleadoServicio _empleados;

    public PermisoController(
        PermisoLogica permisoData,
        Motivo_PermLogica motivoData,
        EmpleadoLogica empleadoData,
        PersonaLogica personaData,
        SecuenciaService secuencia,
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
    public async Task<IActionResult> Index(int? idEmpleado)
    {
        var permisos = (await _permisoData.Listar())
            .Where(p => idEmpleado is null || p.IdEmpleado == idEmpleado)
            .OrderByDescending(p => p.FechaInicio);

        var motivos = await _motivoData.Listar();

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.NombreEmpleado = idEmpleado is null ? null : await NombreEmpleadoAsync(idEmpleado.Value);
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Motivos = motivos;
        return View(permisos.ToList());
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
        var guardo = await _secuencia.EjecutarConBloqueoAsync(
            async () =>
            {
                var lista = await _permisoData.Listar();
                return lista
                    .Where(p => p.IdEmpleado == modelo.IdEmpleado)
                    .Select(p => p.NPermiso)
                    .DefaultIfEmpty(0)
                    .Max() + 1;
            },
            async id =>
            {
                modelo.NPermiso = id;
                await _permisoData.Crear(modelo);
                return true;
            });
        if (!guardo)
            ModelState.AddModelError("", "No se pudo registrar el permiso: intente nuevamente.");
        else
            return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado });

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

    // ---------- Utilidades ----------

    private async Task<List<(int Id, string Nombre)>> EmpleadosAsync()
        => await _empleados.EmpleadosAsync();

    private async Task<string?> NombreEmpleadoAsync(int idEmpleado)
        => await _empleados.NombreEmpleadoAsync(idEmpleado);
}