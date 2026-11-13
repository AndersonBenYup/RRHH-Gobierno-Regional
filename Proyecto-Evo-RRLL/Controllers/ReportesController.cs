using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Evo_RRLL.Controllers;

/// <summary>
/// Centro de reportes: reúne en una pantalla los reportes y exportaciones que ya existen en cada módulo
/// (como el menú Reportes del sistema de escritorio). No genera datos propios; cada enlace se muestra
/// solo si el usuario puede abrir el módulo de destino (User.PuedeVer en la vista).
/// </summary>
[Authorize]
public class ReportesController : Controller
{
    [HttpGet]
    public IActionResult Index() => View();
}
