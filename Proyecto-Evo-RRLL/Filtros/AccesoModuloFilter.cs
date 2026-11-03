using Capa_Logica.Seguridad;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Proyecto_Evo_RRLL.Seguridad;

namespace Proyecto_Evo_RRLL.Filtros;

/// <summary>SC-009: si el rol del usuario no tiene el módulo (Rol_Acceso), responde 403 con una página clara.</summary>
public sealed class AccesoModuloFilter : IAuthorizationFilter
{
    private readonly ILogger<AccesoModuloFilter> _logger;

    public AccesoModuloFilter(ILogger<AccesoModuloFilter> logger) => _logger = logger;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var usuario = context.HttpContext.User;
        if (usuario.Identity?.IsAuthenticated != true)
            return;

        var controller = context.RouteData.Values["controller"]?.ToString();
        if (controller is null || usuario.PuedeVer(controller))
            return;

        var modulo = ReglasAcceso.ModuloDe(controller)!;
        _logger.LogWarning("{Usuario} intentó abrir {Modulo} sin permiso", usuario.Identity.Name, modulo.Nombre);
        context.Result = new ViewResult
        {
            ViewName = "SinAcceso",
            StatusCode = StatusCodes.Status403Forbidden,
            ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), context.ModelState)
            {
                ["Title"] = "Sin acceso",
                ["Modulo"] = modulo.Nombre
            }
        };
    }
}
