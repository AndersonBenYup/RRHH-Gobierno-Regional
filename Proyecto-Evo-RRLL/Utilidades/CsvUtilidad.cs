namespace Proyecto_Evo_RRLL.Utilidades;

internal static class CsvUtilidad
{
    // Conserva el escape de campos utilizado por las exportaciones CSV del sistema.
    internal static string EscaparCsv(string valor)
    {
        if (valor.Contains(';') || valor.Contains('"') || valor.Contains('\n') || valor.Contains('\r'))
            return $"\"{valor.Replace("\"", "\"\"")}\"";
        return valor;
    }
}
