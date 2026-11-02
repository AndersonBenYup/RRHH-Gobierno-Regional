using System.ComponentModel.DataAnnotations;

namespace Capa_Entidades.Validacion;

/// <summary>
/// SC-008: la fecha marcada no puede ser anterior a la de otra propiedad (por ejemplo FechaFin ≥ FechaInicio).
/// Funciona con DateTime, DateOnly y sus versiones anulables. Si alguna de las dos está vacía no valida.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class FechaNoAnteriorAAttribute : ValidationAttribute
{
    public string OtraPropiedad { get; }

    public FechaNoAnteriorAAttribute(string otraPropiedad)
        : base("La fecha no puede ser anterior a la fecha de inicio.")
    {
        OtraPropiedad = otraPropiedad;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
            return ValidationResult.Success;

        var propiedad = validationContext.ObjectType.GetProperty(OtraPropiedad);
        var otra = propiedad?.GetValue(validationContext.ObjectInstance);
        if (otra is null)
            return ValidationResult.Success;

        if (value is IComparable actual && value.GetType() == otra.GetType() && actual.CompareTo(otra) < 0)
        {
            var miembros = validationContext.MemberName is null ? null : new[] { validationContext.MemberName };
            return new ValidationResult(ErrorMessageString, miembros);
        }

        return ValidationResult.Success;
    }
}
