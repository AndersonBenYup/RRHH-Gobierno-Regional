using Capa_Entidades.Validacion;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Proyecto_Evo_RRLL.Utilidades;

/// <summary>SC-008: pasa los errores de las reglas de negocio (Capa Lógica) al ModelState de MVC.</summary>
public static class ModelStateExtensiones
{
    /// <param name="prefijo">Prefijo del modelo en el formulario (por ejemplo "Persona" en Persona.NumDocID).</param>
    public static void AgregarErrores(this ModelStateDictionary modelState, IEnumerable<ErrorValidacion> errores, string prefijo = "")
    {
        foreach (var error in errores)
        {
            var clave = string.IsNullOrEmpty(error.Campo) || string.IsNullOrEmpty(prefijo)
                ? error.Campo
                : $"{prefijo}.{error.Campo}";
            modelState.AddModelError(clave, error.Mensaje);
        }
    }
}
