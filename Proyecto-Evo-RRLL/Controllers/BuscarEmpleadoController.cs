using Capa_Logica;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Evo_RRLL.Controllers;

/// <summary>
/// SC-031: apoyo de los desplegables de empleado con buscador (wwwroot/js/site.js, initSelectBuscable).
/// No pertenece a ningún módulo: lo usan los formularios de todos. Solo devuelve Id de empleado, nunca documentos.
/// </summary>
[Authorize]
public class BuscarEmpleadoController : Controller
{
    private readonly BusquedaEmpleadoLogica _busqueda;

    public BuscarEmpleadoController(BusquedaEmpleadoLogica busqueda) => _busqueda = busqueda;

    [HttpGet]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> PorDocumento(string? numero)
        => Json(await _busqueda.PorDocumento(numero));
}
