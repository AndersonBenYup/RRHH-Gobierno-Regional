using Capa_Logica;
using Proyecto_Evo_RRLL.Utilidades;
using Capa_Datos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Evo_RRLL.Models.ViewModels;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class CatalogoController : Controller
{
    private readonly CatalogoRepositorio _repositorio;
    private readonly BitacoraLogica _bitacora;

    public CatalogoController(CatalogoRepositorio repositorio, BitacoraLogica bitacora)
    {
        _repositorio = repositorio;
        _bitacora = bitacora;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string tabla)
    {
        if (!CatalogoNavegacion.Items.ContainsKey(tabla))
            return NotFound();

        var columnas = _repositorio.Columnas(tabla);
        var filas = new List<IReadOnlyDictionary<string, object?>>();

        foreach (var entidad in await _repositorio.Listar(tabla))
        {
            var fila = new Dictionary<string, object?>();
            foreach (var col in columnas)
            {
                var valor = entidad.GetType().GetProperty(col.Nombre)?.GetValue(entidad);
                fila[col.Nombre] = valor is null ? null : valor.ToString();
            }
            filas.Add(fila);
        }

        return View(new CatalogoViewModel
        {
            Tabla = tabla,
            Titulo = CatalogoNavegacion.Items[tabla],
            Columnas = columnas,
            Filas = filas,
            Claves = _repositorio.ClavesPrimarias(tabla),
            Editable = CatalogoRepositorio.EsEditable(tabla)
        });
    }

    [HttpGet]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Nuevo(string tabla)
    {
        if (!EsCatalogoEditable(tabla))
            return NotFound();

        return View(new CatalogoViewModel
        {
            Tabla = tabla,
            Titulo = $"Nuevo - {CatalogoNavegacion.Items[tabla]}",
            Columnas = _repositorio.Columnas(tabla),
            EsFormulario = true,
            Valores = new Dictionary<string, object?>()
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Nuevo(string tabla, IFormCollection form)
    {
        if (!EsCatalogoEditable(tabla))
            return NotFound();

        var datos = FormAAccion(form);
        try
        {
            await _repositorio.Crear(tabla, datos);
            TempData["MensajeExito"] = $"{CatalogoNavegacion.Items[tabla]}: registro creado correctamente.";
            await _bitacora.Registrar(User.Identity?.Name ?? "?", "Crear", CatalogoNavegacion.Items[tabla], string.Join(" | ", datos.Select(d => $"{d.Key}={d.Value}")));
        }
        catch (Exception ex)
        {
            TempData["MensajeError"] = $"No se pudo crear el registro: {ex.Message}";
        }
        return RedirectToAction("Index", new { tabla });
    }

    [HttpGet]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Editar(string tabla, string ids)
    {
        if (!EsCatalogoEditable(tabla))
            return NotFound();

        var separados = ids.Split('|');
        var entidad = await _repositorio.Obtener(tabla, separados);
        if (entidad is null)
            return NotFound();

        var columnas = _repositorio.Columnas(tabla);
        var valores = new Dictionary<string, object?>();
        foreach (var col in columnas)
            valores[col.Nombre] = entidad.GetType().GetProperty(col.Nombre)?.GetValue(entidad);

        return View("Nuevo", new CatalogoViewModel
        {
            Tabla = tabla,
            Titulo = $"Editar - {CatalogoNavegacion.Items[tabla]}",
            Columnas = columnas,
            Claves = _repositorio.ClavesPrimarias(tabla),
            EsFormulario = true,
            Valores = valores
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Editar(string tabla, string ids, IFormCollection form)
    {
        if (!EsCatalogoEditable(tabla))
            return NotFound();

        var separados = ids.Split('|');
        var datos = FormAAccion(form);
        try
        {
            await _repositorio.Actualizar(tabla, separados, datos);
            TempData["MensajeExito"] = $"{CatalogoNavegacion.Items[tabla]}: registro actualizado correctamente.";
            await _bitacora.Registrar(User.Identity?.Name ?? "?", "Editar", CatalogoNavegacion.Items[tabla], $"clave(s)={ids}");
        }
        catch (Exception ex)
        {
            TempData["MensajeError"] = $"No se pudo actualizar el registro: {ex.Message}";
        }
        return RedirectToAction("Index", new { tabla });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Eliminar(string tabla, string ids)
    {
        if (!EsCatalogoEditable(tabla))
            return NotFound();

        try
        {
            await _repositorio.Eliminar(tabla, ids.Split('|'));
            TempData["MensajeExito"] = $"{CatalogoNavegacion.Items[tabla]}: registro eliminado correctamente.";
            await _bitacora.Registrar(User.Identity?.Name ?? "?", "Eliminar", CatalogoNavegacion.Items[tabla], $"clave(s)={ids}");
        }
        catch (Exception ex)
        {
            TempData["MensajeError"] = $"No se pudo eliminar el registro: {ex.Message}";
        }
        return RedirectToAction("Index", new { tabla });
    }

    private static IReadOnlyDictionary<string, string> FormAAccion(IFormCollection form)
    {
        var dict = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var clave in form.Keys)
            dict[clave] = form[clave].ToString();
        return dict;
    }

    private static bool EsCatalogoEditable(string tabla)
        => CatalogoNavegacion.Items.ContainsKey(tabla) && CatalogoRepositorio.EsEditable(tabla);

    [HttpGet]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Exportar(string tabla)
    {
        if (!CatalogoNavegacion.Items.ContainsKey(tabla))
            return NotFound();

        var columnas = _repositorio.Columnas(tabla);
        var filas = new List<string[]>
        {
            columnas.Select(c => c.Nombre).ToArray()
        };

        foreach (var entidad in await _repositorio.Listar(tabla))
        {
            filas.Add(columnas.Select(c => entidad.GetType().GetProperty(c.Nombre)?.GetValue(entidad)?.ToString() ?? string.Empty).ToArray());
        }

        var contenido = string.Join("\r\n", filas.Select(f => string.Join(";", f.Select(CsvUtilidad.EscaparCsv))));
        var nombre = $"{tabla}_{DateTime.Now:yyyyMMdd_HHmm}.csv";
        return File(new System.Text.UTF8Encoding(true).GetBytes(contenido), "text/csv; charset=utf-8", nombre);
    }

}