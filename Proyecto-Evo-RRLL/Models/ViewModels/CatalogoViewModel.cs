using Capa_Datos;

namespace Proyecto_Evo_RRLL.Models.ViewModels;

public class CatalogoViewModel
{
    public string Tabla { get; set; } = string.Empty;

    public string Titulo { get; set; } = string.Empty;

    public IReadOnlyList<CatalogoColumna> Columnas { get; set; } = Array.Empty<CatalogoColumna>();

    public IReadOnlyList<IReadOnlyDictionary<string, object?>> Filas { get; set; } = Array.Empty<IReadOnlyDictionary<string, object?>>();

    public string[] Claves { get; set; } = Array.Empty<string>();

    public bool Editable { get; set; }

    public bool EsFormulario { get; set; }

    public IReadOnlyDictionary<string, object?>? Valores { get; set; }
}