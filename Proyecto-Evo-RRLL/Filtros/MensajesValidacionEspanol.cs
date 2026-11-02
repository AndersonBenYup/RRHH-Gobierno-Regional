using Capa_Entidades.Validacion;
using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Evo_RRLL.Filtros;

/// <summary>
/// SC-008: pone en español los mensajes que MVC genera al convertir los valores del formulario
/// (por ejemplo "The value 'x' is not valid"). Los [Required] de las entidades ya traen su propio texto.
/// </summary>
public static class MensajesValidacionEspanol
{
    public static void Configurar(MvcOptions opciones)
    {
        var m = opciones.ModelBindingMessageProvider;
        m.SetValueMustNotBeNullAccessor(_ => Mensajes.Obligatorio);
        m.SetMissingBindRequiredValueAccessor(_ => Mensajes.Obligatorio);
        m.SetMissingKeyOrValueAccessor(() => Mensajes.Obligatorio);
        m.SetValueIsInvalidAccessor(v => $"El valor «{v}» no es válido.");
        m.SetAttemptedValueIsInvalidAccessor((v, _) => $"El valor «{v}» no es válido.");
        m.SetUnknownValueIsInvalidAccessor(_ => "El valor ingresado no es válido.");
        m.SetNonPropertyAttemptedValueIsInvalidAccessor(v => $"El valor «{v}» no es válido.");
        m.SetNonPropertyUnknownValueIsInvalidAccessor(() => "El valor ingresado no es válido.");
        m.SetValueMustBeANumberAccessor(_ => "Ingrese un número.");
        m.SetNonPropertyValueMustBeANumberAccessor(() => "Ingrese un número.");
    }
}
