namespace Capa_Logica.Paginacion;

/// <summary>
/// SC-005: qué números muestra el paginador. Como máximo 7 elementos entre "Anterior" y "Siguiente":
/// la primera página, la última, la actual con sus vecinas y "…" (0) donde se saltan páginas.
/// Ejemplo con 120 páginas y la 53 actual: 1 … 52 53 54 … 120.
/// wwwroot/js/site.js repite este mismo cálculo para las tablas que se paginan en el navegador.
/// </summary>
public static class VentanaPaginas
{
    public const int Elipsis = 0;
    public const int MaximoElementos = 7;

    /// <summary>Tamaños de página que se pueden elegir en el selector "Filas por página".</summary>
    public static readonly IReadOnlyList<int> TamanosPermitidos = [10, 25, 50, 100];

    public static IReadOnlyList<int> Calcular(int paginaActual, int totalPaginas)
    {
        if (totalPaginas <= 0) return [];
        var actual = Math.Clamp(paginaActual, 1, totalPaginas);

        if (totalPaginas <= MaximoElementos)
            return Enumerable.Range(1, totalPaginas).ToArray();

        // Cerca del inicio: 1 2 3 4 5 … N
        if (actual <= 4)
            return [1, 2, 3, 4, 5, Elipsis, totalPaginas];

        // Cerca del final: 1 … N-4 N-3 N-2 N-1 N
        if (actual >= totalPaginas - 3)
            return [1, Elipsis, totalPaginas - 4, totalPaginas - 3, totalPaginas - 2, totalPaginas - 1, totalPaginas];

        // En medio: 1 … a-1 a a+1 … N
        return [1, Elipsis, actual - 1, actual, actual + 1, Elipsis, totalPaginas];
    }

    /// <summary>Devuelve el tamaño pedido si está permitido; si no, el valor por defecto.</summary>
    public static int NormalizarTamano(int? tamano, int porDefecto)
        => tamano is int t && TamanosPermitidos.Contains(t) ? t : porDefecto;
}
