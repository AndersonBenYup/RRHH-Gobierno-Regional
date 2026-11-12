namespace Capa_Logica.Reglas;

/// <summary>SC-031: reglas de la búsqueda de empleados por número de documento. Funciones puras, sin BD.</summary>
public static class ReglasBusquedaEmpleado
{
    /// <summary>Dígitos mínimos para buscar: con menos, casi todos los documentos coinciden.</summary>
    public const int DigitosMinimos = 3;

    /// <summary>Máximo de empleados que devuelve una búsqueda.</summary>
    public const int MaximoResultados = 50;

    /// <summary>
    /// Deja solo los dígitos de lo escrito (se aceptan espacios, puntos o guiones: «12.345-678»).
    /// Devuelve null si no llega al mínimo de dígitos o si hay letras (entonces es una búsqueda por nombre).
    /// </summary>
    public static string? NormalizarDocumento(string? texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            return null;
        var limpio = texto.Trim();
        if (limpio.Any(char.IsLetter))
            return null;
        var digitos = new string(limpio.Where(char.IsAsciiDigit).ToArray());
        return digitos.Length >= DigitosMinimos ? digitos : null;
    }
}
