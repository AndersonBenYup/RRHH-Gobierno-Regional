namespace Capa_Logica.Seguridad;

/// <summary>SC-009: política mínima de contraseñas para las cuentas del sistema.</summary>
public static class PoliticaClave
{
    public const int LargoMinimo = 8;
    public const string Descripcion = "Mínimo 8 caracteres, con al menos una letra y un número.";

    /// <returns>Lista de incumplimientos; vacía si la clave es aceptable.</returns>
    public static List<string> Validar(string? clave, string? login = null)
    {
        var errores = new List<string>();
        if (string.IsNullOrEmpty(clave))
        {
            errores.Add("La clave es obligatoria.");
            return errores;
        }

        if (clave.Length < LargoMinimo)
            errores.Add($"La clave debe tener al menos {LargoMinimo} caracteres.");
        if (!clave.Any(char.IsLetter))
            errores.Add("La clave debe incluir al menos una letra.");
        if (!clave.Any(char.IsDigit))
            errores.Add("La clave debe incluir al menos un número.");
        if (!string.IsNullOrWhiteSpace(login) && clave.Contains(login.Trim(), StringComparison.OrdinalIgnoreCase))
            errores.Add("La clave no puede contener el nombre de usuario.");

        return errores;
    }
}
