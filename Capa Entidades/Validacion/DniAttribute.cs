using System.ComponentModel.DataAnnotations;

namespace Capa_Entidades.Validacion;

/// <summary>
/// SC-008: valida que un DNI peruano tenga exactamente 8 dígitos.
/// Hereda de RegularExpressionAttribute para que MVC genere también la validación en el navegador.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class DniAttribute : RegularExpressionAttribute
{
    public const string Patron = @"^\d{8}$";
    public const string Mensaje = "El DNI debe tener exactamente 8 dígitos.";

    public DniAttribute() : base(Patron)
    {
        ErrorMessage = Mensaje;
    }

    /// <summary>Comprueba un DNI (se ignoran los espacios de relleno de las columnas char).</summary>
    public static bool EsValido(string? dni)
        => dni is not null && System.Text.RegularExpressions.Regex.IsMatch(dni.Trim(), Patron);

    public override bool IsValid(object? value)
        => value is null || (value is string s && (s.Trim().Length == 0 || EsValido(s)));
}
