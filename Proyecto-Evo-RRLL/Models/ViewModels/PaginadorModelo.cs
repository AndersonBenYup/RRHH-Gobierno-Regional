using System.Globalization;
using Capa_Logica.Paginacion;

namespace Proyecto_Evo_RRLL.Models.ViewModels;

/// <summary>
/// SC-005: datos del paginador del servidor (Views/Shared/_Paginador.cshtml), reutilizable en cualquier tabla.
/// <paramref name="Ruta"/> lleva los filtros actuales (búsqueda, empleado…) para conservarlos al cambiar de página.
/// </summary>
public sealed record PaginadorModelo(
    int Pagina,
    int TotalPaginas,
    int Total,
    string Accion,
    IReadOnlyDictionary<string, string?> Ruta,
    int TamanoPagina = 25,
    string Etiqueta = "registros")
{
    private static readonly CultureInfo Peru = CultureInfo.GetCultureInfo("es-PE");

    public static IReadOnlyList<int> TamanosPermitidos => VentanaPaginas.TamanosPermitidos;

    public int Desde => Total == 0 ? 0 : (Pagina - 1) * TamanoPagina + 1;
    public int Hasta => Math.Min(Pagina * TamanoPagina, Total);
    public bool HayAnterior => Pagina > 1;
    public bool HaySiguiente => Pagina < TotalPaginas;

    /// <summary>Números a mostrar (0 = "…"); como máximo 7.</summary>
    public IReadOnlyList<int> Ventana() => VentanaPaginas.Calcular(Pagina, TotalPaginas);

    public Dictionary<string, string?> RutaPara(int pagina)
        => new(Ruta) { ["pagina"] = pagina.ToString(), ["tamano"] = TamanoPagina.ToString() };

    /// <summary>Filtros actuales, sin página ni tamaño, para los formularios "Filas por página" e "Ir a página".</summary>
    public IEnumerable<KeyValuePair<string, string?>> Filtros
        => Ruta.Where(r => r.Key is not ("pagina" or "tamano") && !string.IsNullOrEmpty(r.Value));

    public static string Numero(int n) => n.ToString("N0", Peru);
}
