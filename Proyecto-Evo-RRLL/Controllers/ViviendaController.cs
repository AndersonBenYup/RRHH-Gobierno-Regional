using Capa_Logica;
using Capa_Datos;
using Capa_Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Evo_RRLL.Models.ViewModels;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class ViviendaController : Controller
{
    private readonly EmpleadoLogica _empleadoData;
    private readonly PersonaLogica _personaData;
    private readonly ViviendaLogica _viviendaData;
    private readonly TipoVivLogica _tipoVivData;
    private readonly TenenciaVivLogica _tenenciaVivData;
    private readonly UbicacionVivLogica _ubicacionVivData;
    private readonly AguaVivLogica _aguaVivData;
    private readonly AlumbradoVivLogica _alumbradoVivData;
    private readonly ConservacionVivLogica _conservacionVivData;
    private readonly ExcretasVivLogica _excretasVivData;
    private readonly MaterialVivLogica _materialVivData;
    private readonly ServicioVivLogica _servicioVivData;
    private readonly ViviendaServicioLogica _viviendaServicioData;
    private readonly SecuenciaService _secuencia;
    private readonly Services.EmpleadoServicio _empleados;

    public ViviendaController(
        EmpleadoLogica empleadoData,
        PersonaLogica personaData,
        ViviendaLogica viviendaData,
        TipoVivLogica tipoVivData,
        TenenciaVivLogica tenenciaVivData,
        UbicacionVivLogica ubicacionVivData,
        AguaVivLogica aguaVivData,
        AlumbradoVivLogica alumbradoVivData,
        ConservacionVivLogica conservacionVivData,
        ExcretasVivLogica excretasVivData,
        MaterialVivLogica materialVivData,
        ServicioVivLogica servicioVivData,
        ViviendaServicioLogica viviendaServicioData,
        SecuenciaService secuencia,
        Services.EmpleadoServicio empleados)
    {
        _empleadoData = empleadoData;
        _personaData = personaData;
        _viviendaData = viviendaData;
        _tipoVivData = tipoVivData;
        _tenenciaVivData = tenenciaVivData;
        _ubicacionVivData = ubicacionVivData;
        _aguaVivData = aguaVivData;
        _alumbradoVivData = alumbradoVivData;
        _conservacionVivData = conservacionVivData;
        _excretasVivData = excretasVivData;
        _materialVivData = materialVivData;
        _servicioVivData = servicioVivData;
        _viviendaServicioData = viviendaServicioData;
        _secuencia = secuencia;
        _empleados = empleados;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? idEmpleado)
    {
        ViewBag.Empleados = await EmpleadosAsync();

        if (idEmpleado is null)
            return View("Indice", ViewBag.Empleados);

        int id = idEmpleado.Value;
        if (await _empleadoData.Obtener(id) is null)
            return NotFound();

        var registro = (await _viviendaData.Listar()).FirstOrDefault(v => v.IdEmpleado == id);

        var tipos = await _tipoVivData.Listar();
        var tenencias = await _tenenciaVivData.Listar();
        var ubicaciones = await _ubicacionVivData.Listar();
        var aguas = await _aguaVivData.Listar();
        var alumbrados = await _alumbradoVivData.Listar();
        var conservaciones = await _conservacionVivData.Listar();
        var excretas = await _excretasVivData.Listar();
        var materiales = await _materialVivData.Listar();

        var vm = new ViviendaViewModel
        {
            IdEmpleado = id,
            NombreEmpleado = await NombreEmpleadoAsync(id),
            Registro = registro,
            Tenencia = registro?.IdTenencia is null ? null : tenencias.FirstOrDefault(t => t.IdTenencia == registro.IdTenencia)?.DescripTenencia,
            Tipo = registro?.IdTipo is null ? null : tipos.FirstOrDefault(t => t.IdTipo == registro.IdTipo)?.DescripTipo,
            Material = registro?.IdMaterial is null ? null : materiales.FirstOrDefault(m => m.IdMaterial == registro.IdMaterial)?.DescripMaterial,
            Conservacion = registro?.IdConservacion is null ? null : conservaciones.FirstOrDefault(c => c.IdConservacion == registro.IdConservacion)?.DescripConserva,
            Ubicacion = registro?.IdUbicacion is null ? null : ubicaciones.FirstOrDefault(u => u.IdUbicacion == registro.IdUbicacion)?.DescripUbicacion,
            Alumbrado = registro?.IdAlumbrado is null ? null : alumbrados.FirstOrDefault(a => a.IdAlumbrado == registro.IdAlumbrado)?.DescripAlumbrado,
            Agua = registro?.IdAgua is null ? null : aguas.FirstOrDefault(a => a.IdAgua == registro.IdAgua)?.DescripAgua,
            Excretas = registro?.IdExcretas is null ? null : excretas.FirstOrDefault(e => e.IdExcretas == registro.IdExcretas)?.DescripExcretas,
            Servicios = await ServiciosAsync(registro?.IdVivienda)
        };

        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> ViviendaForm(int idEmpleado)
    {
        if (await _empleadoData.Obtener(idEmpleado) is null)
            return NotFound();

        var registro = (await _viviendaData.Listar()).FirstOrDefault(v => v.IdEmpleado == idEmpleado) ?? new Vivienda();

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.NombreEmpleado = await NombreEmpleadoAsync(idEmpleado);
        ViewBag.Tipos = await _tipoVivData.Listar();
        ViewBag.Tenencias = await _tenenciaVivData.Listar();
        ViewBag.Ubicaciones = await _ubicacionVivData.Listar();
        ViewBag.Aguas = await _aguaVivData.Listar();
        ViewBag.Alumbrados = await _alumbradoVivData.Listar();
        ViewBag.Conservaciones = await _conservacionVivData.Listar();
        ViewBag.Excretas = await _excretasVivData.Listar();
        ViewBag.Materiales = await _materialVivData.Listar();
        ViewBag.Servicios = await ServiciosAsync(registro.IdVivienda == 0 ? null : registro.IdVivienda);
        return View(registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ViviendaForm(Vivienda modelo, int[] servicios)
    {
        var actual = (await _viviendaData.Listar()).FirstOrDefault(v => v.IdEmpleado == modelo.IdEmpleado);

        if (actual is null)
        {
            var guardo = await _secuencia.EjecutarTransaccionalAsync(async () =>
            {
                modelo.IdVivienda = 0;
                await _viviendaData.Crear(modelo);
                await GuardarServiciosAsync(modelo.IdVivienda, servicios);
            });
            if (!guardo)
            {
                ViewBag.IdEmpleado = modelo.IdEmpleado;
                ViewBag.NombreEmpleado = await NombreEmpleadoAsync(modelo.IdEmpleado ?? 0);
                ViewBag.Tipos = await _tipoVivData.Listar();
                ViewBag.Tenencias = await _tenenciaVivData.Listar();
                ViewBag.Ubicaciones = await _ubicacionVivData.Listar();
                ViewBag.Aguas = await _aguaVivData.Listar();
                ViewBag.Alumbrados = await _alumbradoVivData.Listar();
                ViewBag.Conservaciones = await _conservacionVivData.Listar();
                ViewBag.Excretas = await _excretasVivData.Listar();
                ViewBag.Materiales = await _materialVivData.Listar();
                ViewBag.Servicios = (await _servicioVivData.Listar())
                    .Select(s => (Servicio: s, Tiene: servicios.Contains(s.IdServicio))).ToList();
                ModelState.AddModelError("", "No se pudo guardar la vivienda: intente nuevamente.");
                return View(modelo);
            }
        }
        else
        {
            Copiar(actual, modelo);
            await _viviendaData.Actualizar(actual);
            await GuardarServiciosAsync(actual.IdVivienda, servicios);
        }

        return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int idEmpleado)
    {
        var actual = (await _viviendaData.Listar()).FirstOrDefault(v => v.IdEmpleado == idEmpleado);
        if (actual is not null)
            await _viviendaData.Eliminar(actual);
        return RedirectToAction(nameof(Index), new { idEmpleado });
    }

    // ---------- Utilidades ----------

    private async Task<List<(int Id, string Nombre)>> EmpleadosAsync()
        => await _empleados.EmpleadosAsync();

    private async Task<string?> NombreEmpleadoAsync(int idEmpleado)
        => await _empleados.NombreEmpleadoAsync(idEmpleado);

    private async Task<List<(ServicioViv Servicio, bool Tiene)>> ServiciosAsync(int? idVivienda)
    {
        var marcados = idVivienda is null
            ? new HashSet<int>()
            : (await _viviendaServicioData.Listar()).Where(s => s.IdVivienda == idVivienda).Select(s => s.IdServicio).ToHashSet();
        return (await _servicioVivData.Listar())
            .OrderBy(s => s.IdServicio)
            .Select(s => (s, marcados.Contains(s.IdServicio)))
            .ToList();
    }

    private async Task GuardarServiciosAsync(int idVivienda, int[] servicios)
    {
        var actuales = (await _viviendaServicioData.Listar()).Where(s => s.IdVivienda == idVivienda).ToList();
        foreach (var s in actuales.Where(s => !servicios.Contains(s.IdServicio)))
            await _viviendaServicioData.Eliminar(s);
        foreach (var id in servicios.Distinct().Where(id => actuales.All(s => s.IdServicio != id)))
            await _viviendaServicioData.Crear(new ViviendaServicio { IdVivienda = idVivienda, IdServicio = id });
    }

    private static void Copiar(Vivienda destino, Vivienda origen)
    {
        destino.IdEmpleado = origen.IdEmpleado;
        destino.IdTenencia = origen.IdTenencia;
        destino.IdTipo = origen.IdTipo;
        destino.IdMaterial = origen.IdMaterial;
        destino.IdConservacion = origen.IdConservacion;
        destino.NumDormitorio = origen.NumDormitorio;
        destino.NumServHig = origen.NumServHig;
        destino.IdUbicacion = origen.IdUbicacion;
        destino.IdAlumbrado = origen.IdAlumbrado;
        destino.OtroAlumbrado = origen.OtroAlumbrado;
        destino.IdAgua = origen.IdAgua;
        destino.OtroAgua = origen.OtroAgua;
        destino.IdExcretas = origen.IdExcretas;
        destino.OtroExcreta = origen.OtroExcreta;
        destino.NumCompart = origen.NumCompart;
        destino.Observacion = origen.Observacion;
        destino.NumPerDorm = origen.NumPerDorm;
    }
}