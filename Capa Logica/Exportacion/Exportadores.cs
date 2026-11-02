using System.Text;

namespace Capa_Logica.Exportacion;

/// <summary>Resultado de una exportación: el contenido y cómo entregarlo al navegador.</summary>
public sealed record ArchivoExportado(byte[] Contenido, string TipoContenido, string NombreArchivo);

/// <summary>
/// SC-003 (diseño OCP de la Semana 5): cada formato de exportación es una clase. Para agregar uno nuevo
/// (por ejemplo Excel) se crea otra implementación y se registra; no se toca el código que exporta.
/// </summary>
public interface IExportadorTabla
{
    /// <summary>Clave del formato ("csv").</summary>
    string Formato { get; }
    string TipoContenido { get; }
    string Extension { get; }

    /// <param name="filas">La primera fila es la cabecera.</param>
    byte[] Exportar(IReadOnlyList<string?[]> filas);
}

/// <summary>CSV con punto y coma y BOM UTF-8, para que Excel en español lo abra con tildes y columnas correctas.</summary>
public sealed class ExportadorCsv : IExportadorTabla
{
    public string Formato => "csv";
    public string TipoContenido => "text/csv; charset=utf-8";
    public string Extension => "csv";

    public byte[] Exportar(IReadOnlyList<string?[]> filas)
    {
        var texto = string.Join("\r\n", filas.Select(f => string.Join(";", f.Select(Escapar))));
        return new UTF8Encoding(encoderShouldEmitUTF8Identifier: true).GetPreamble()
            .Concat(Encoding.UTF8.GetBytes(texto)).ToArray();
    }

    /// <summary>Encierra entre comillas los valores con ; comillas o saltos de línea y duplica las comillas.</summary>
    public static string Escapar(string? valor)
    {
        valor ??= string.Empty;
        // Evita la inyección de fórmulas al abrir el archivo en Excel.
        if (valor.Length > 0 && "=+-@".Contains(valor[0]) && !double.TryParse(valor, out _))
            valor = "'" + valor;
        if (valor.Contains(';') || valor.Contains('"') || valor.Contains('\n') || valor.Contains('\r'))
            return $"\"{valor.Replace("\"", "\"\"")}\"";
        return valor;
    }
}

/// <summary>Elige el exportador por formato. Los formatos disponibles son los exportadores registrados.</summary>
public sealed class ExportadorTablas
{
    private readonly Dictionary<string, IExportadorTabla> _exportadores;

    public ExportadorTablas(IEnumerable<IExportadorTabla> exportadores)
        => _exportadores = exportadores.ToDictionary(e => e.Formato, StringComparer.OrdinalIgnoreCase);

    public IReadOnlyCollection<string> Formatos => _exportadores.Keys;

    /// <param name="nombreBase">Nombre del archivo sin extensión; se le agrega la fecha y hora.</param>
    public ArchivoExportado Exportar(IReadOnlyList<string?[]> filas, string nombreBase, string formato = "csv")
    {
        if (!_exportadores.TryGetValue(formato, out var exportador))
            throw new ArgumentException($"Formato de exportación no soportado: {formato}", nameof(formato));
        var nombre = $"{nombreBase}_{DateTime.Now:yyyyMMdd_HHmm}.{exportador.Extension}";
        return new ArchivoExportado(exportador.Exportar(filas), exportador.TipoContenido, nombre);
    }
}
