namespace Proyecto_Evo_RRLL.Models.ViewModels;

/// <summary>SC-005: datos del paginador del servidor (Views/Shared/_Paginador.cshtml).</summary>
public sealed record PaginadorModelo(
    int Pagina,
    int TotalPaginas,
    int Total,
    string Accion,
    IReadOnlyDictionary<string, string?> Ruta)
{
    /// <summary>Páginas a mostrar: la primera, la última y dos a cada lado de la actual (0 = "…").</summary>
    public IEnumerable<int> Ventana()
    {
        var anterior = 0;
        for (var p = 1; p <= TotalPaginas; p++)
        {
            if (p == 1 || p == TotalPaginas || Math.Abs(p - Pagina) <= 2)
            {
                if (anterior != 0 && p - anterior > 1)
                    yield return 0;
                yield return p;
                anterior = p;
            }
        }
    }

    public Dictionary<string, string?> RutaPara(int pagina)
        => new(Ruta) { ["pagina"] = pagina.ToString() };
}
