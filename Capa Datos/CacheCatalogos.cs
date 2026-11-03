using Microsoft.Extensions.Caching.Memory;

namespace Capa_Datos;

/// <summary>
/// SC-005: caché en memoria de los catálogos pequeños (cargos, AFP, horarios, tipos…), que casi no cambian
/// y se leen en cada formulario. Se invalida al crear, editar o eliminar un registro del catálogo, y además
/// vence a los <see cref="Duracion"/> minutos por si otra instancia de la app cambió los datos.
/// </summary>
public sealed class CacheCatalogos
{
    public static readonly TimeSpan Duracion = TimeSpan.FromMinutes(10);

    /// <summary>Tablas que se guardan en caché: los catálogos editables y otras tablas de consulta pequeñas.</summary>
    public static readonly IReadOnlySet<string> Tablas = new HashSet<string>(
        CatalogoRepositorio.CatalogoEditable().Concat(["Local", "EstructOrganiz", "Rol", "SistemaOpcion"]),
        StringComparer.Ordinal);

    private readonly IMemoryCache _cache;

    public CacheCatalogos(IMemoryCache cache) => _cache = cache;

    public static bool EsCatalogo(Type tipo) => Tablas.Contains(tipo.Name);

    private static string Clave(string tabla) => "catalogo:" + tabla;

    /// <summary>Devuelve una copia de la lista guardada (o la carga con <paramref name="cargar"/> la primera vez).</summary>
    public async Task<List<T>> ObtenerAsync<T>(Func<Task<List<T>>> cargar)
    {
        var lista = await _cache.GetOrCreateAsync(Clave(typeof(T).Name), entrada =>
        {
            entrada.AbsoluteExpirationRelativeToNow = Duracion;
            return cargar();
        });
        return [.. lista!];
    }

    public void Invalidar(Type tipo) => _cache.Remove(Clave(tipo.Name));

    public void Invalidar(string tabla) => _cache.Remove(Clave(tabla));
}
