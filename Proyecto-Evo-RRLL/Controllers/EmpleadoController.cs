using Capa_Logica.Exportacion;
using Proyecto_Evo_RRLL.Utilidades;
using Capa_Logica;
using Capa_Entidades.Contratos;
using Capa_Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Evo_RRLL.Models.ViewModels;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class EmpleadoController : Controller
{
    // SC-006: antes recibía 18 dependencias. El listado y los catálogos de la ficha viven en sus servicios (SRP).
    private readonly EmpleadoLogica _empleadoData;
    private readonly PersonaLogica _personaData;
    private readonly Services.EmpleadoFichaServicio _fichas;
    private readonly Services.EmpleadoListadoServicio _listado;
    private readonly Services.EmpleadoCatalogosServicio _catalogos;

    public EmpleadoController(
        EmpleadoLogica empleadoData,
        PersonaLogica personaData,
        Services.EmpleadoFichaServicio fichas,
        Services.EmpleadoListadoServicio listado,
        Services.EmpleadoCatalogosServicio catalogos)
    {
        _empleadoData = empleadoData;
        _personaData = personaData;
        _fichas = fichas;
        _listado = listado;
        _catalogos = catalogos;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? q, int pagina = 1, int? tamano = null)
    {
        var tamanoPagina = Capa_Logica.Paginacion.VentanaPaginas.NormalizarTamano(tamano, 25);
        var filas = await _listado.ListarAsync(q);

        var total = filas.Count;
        var totalPaginas = Math.Max(1, (int)Math.Ceiling(total / (double)tamanoPagina));
        pagina = Math.Clamp(pagina, 1, totalPaginas);

        var vm = new EmpleadoViewModel
        {
            Busqueda = q, Pagina = pagina, TotalPaginas = totalPaginas, Total = total,
            Empleados = filas.Skip((pagina - 1) * tamanoPagina).Take(tamanoPagina).ToList(),
            Paginador = new PaginadorModelo(pagina, totalPaginas, total, nameof(Index),
                new Dictionary<string, string?> { ["q"] = q }, tamanoPagina, "empleados")
        };

        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> Exportar(string? q, [FromServices] ExportadorTablas exportador)
    {
        var filas = await _listado.FilasExportacionAsync(q);

        // SC-003: el formato lo resuelve ExportadorTablas (OCP).
        var archivo = exportador.Exportar(filas, $"empleados_{(string.IsNullOrWhiteSpace(q) ? "todos" : "filtro")}");
        return File(archivo.Contenido, archivo.TipoContenido, archivo.NombreArchivo);
    }

    [HttpGet]
    public async Task<IActionResult> Detalles(int id)
    {
        var empleado = await _fichas.CargarAsync(id);
        if (empleado is null)
            return NotFound();

        var persona = empleado.IdPersona is null ? null : await _personaData.Obtener(empleado.IdPersona.Value);
        var vm = await _catalogos.DetalleAsync(empleado, persona);
        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var empleado = await _fichas.CargarAsync(id);
        if (empleado is null)
            return NotFound();

        await CargarCatalogosEdicion();
        return View(empleado);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, EmpleadoFicha empleado, IFormFile? foto)
    {
        if (id != empleado.IdEmpleado)
            return NotFound();

        if (!ModelState.IsValid)
        {
            await CargarCatalogosEdicion();
            return View(empleado);
        }

        var archivo = foto is not null && foto.Length > 0 ? await LeerArchivo(foto) : null;
        if (await _fichas.ActualizarAsync(empleado, archivo))
            return RedirectToAction(nameof(Detalles), new { id });

        ModelState.AddModelError("", "No se pudo guardar el empleado: intente nuevamente.");
        await CargarCatalogosEdicion();
        return View(empleado);
    }

    [HttpGet]
    public async Task<IActionResult> Nuevo()
    {
        await CargarCatalogosEdicion();
        return View(new EmpleadoNuevoViewModel
        {
            Empleado = new EmpleadoFicha
            {
                Year = DateTime.Now.Year,
                Estado = EmpleadoFicha.EstadoActivo,
                FechaIngreso = DateTime.Today
            }
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Nuevo(EmpleadoNuevoViewModel modelo, IFormFile? foto)
    {
        var persona = modelo.Persona;
        // SC-008: nombres, DNI de 8 dígitos y documento duplicado se validan en la Capa Lógica.
        ModelState.AgregarErrores(await _personaData.Validar(persona), nameof(modelo.Persona));

        if (ModelState.IsValid)
        {
            var archivo = foto is not null && foto.Length > 0 ? await LeerArchivo(foto) : null;
            var idEmpleado = await _fichas.CrearAsync(persona, modelo.Empleado, archivo);
            if (idEmpleado is not null)
            {
                TempData["MensajeExito"] = "Empleado registrado.";
                return RedirectToAction(nameof(Detalles), new { id = idEmpleado });
            }
            ModelState.AddModelError("", "No se pudo registrar el empleado: intente nuevamente.");
        }

        await CargarCatalogosEdicion();
        return View(modelo);
    }

    private async Task CargarCatalogosEdicion()
    {
        var c = await _catalogos.ListarParaFormularioAsync();
        ViewBag.Cargos = c.Cargos;
        ViewBag.Horarios = c.Horarios;
        ViewBag.Locales = c.Locales;
        ViewBag.Areas = c.Areas;
        ViewBag.TiposTrabajador = c.TiposTrabajador;
        ViewBag.Regimenes = c.Regimenes;
        ViewBag.Afps = c.Afps;
        ViewBag.EstadosCiviles = c.EstadosCiviles;
        ViewBag.TiposSangre = c.TiposSangre;
        ViewBag.Discapacidades = c.Discapacidades;
        ViewBag.ViveCon = c.ViveCon;
        ViewBag.Nacionalidades = c.Nacionalidades;
        ViewBag.TiposDocumento = c.TiposDocumento;
    }

    [HttpGet]
    public async Task<IActionResult> MostrarFoto(int id)
    {
        var empleado = await _empleadoData.Obtener(id);
        var persona = empleado?.IdPersona is null ? null : await _personaData.Obtener(empleado.IdPersona.Value);
        if (persona?.Foto is null)
            return NotFound();
        return File(persona.Foto, "image/jpeg");
    }

    private static async Task<byte[]> LeerArchivo(IFormFile archivo)
    {
        using var ms = new MemoryStream();
        await archivo.CopyToAsync(ms);
        return ms.ToArray();
    }
}