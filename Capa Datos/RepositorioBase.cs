using System.Linq.Expressions;
using Capa_Entidades.Contratos;
using Microsoft.EntityFrameworkCore;

namespace Capa_Datos;

/// <summary>
/// SC-006: CRUD genérico con EF Core. Reemplaza las ~75 clases XxxData que repetían el mismo código;
/// se registra como implementación abierta de IRepositorio&lt;&gt; en <see cref="ConfiguracionCapaDatos"/>.
/// </summary>
public class RepositorioBase<T> : IRepositorio<T> where T : class
{
    protected readonly EvoRRLDbContext Contexto;
    private readonly CacheCatalogos? _cache;

    public RepositorioBase(EvoRRLDbContext contexto) => Contexto = contexto;

    /// <summary>Con caché: los catálogos pequeños (<see cref="CacheCatalogos.Tablas"/>) se leen de memoria.</summary>
    public RepositorioBase(EvoRRLDbContext contexto, CacheCatalogos cache) : this(contexto) => _cache = cache;

    protected DbSet<T> Tabla => Contexto.Set<T>();

    private bool UsaCache => _cache is not null && CacheCatalogos.EsCatalogo(typeof(T));

    public virtual async Task<List<T>> Listar()
        => UsaCache
            ? await _cache!.ObtenerAsync(() => Tabla.AsNoTracking().ToListAsync())
            : await Tabla.ToListAsync();

    public virtual async Task<Pagina<T>> ListarPagina(Expression<Func<T, bool>>? filtro,
        Func<IQueryable<T>, IOrderedQueryable<T>> ordenar, int pagina, int tamanoPagina)
    {
        tamanoPagina = Math.Clamp(tamanoPagina, 1, 500);
        var consulta = Tabla.AsNoTracking();
        if (filtro is not null)
            consulta = consulta.Where(filtro);

        var total = await consulta.CountAsync();
        var totalPaginas = Math.Max(1, (int)Math.Ceiling(total / (double)tamanoPagina));
        pagina = Math.Clamp(pagina, 1, totalPaginas);

        var registros = await ordenar(consulta).Skip((pagina - 1) * tamanoPagina).Take(tamanoPagina).ToListAsync();
        return new Pagina<T>(registros, total, pagina, tamanoPagina);
    }

    public virtual async Task<T?> Obtener(params object[] claves) => await Tabla.FindAsync(claves);

    public virtual async Task Crear(T registro)
    {
        Tabla.Add(registro);
        await Contexto.SaveChangesAsync();
        _cache?.Invalidar(typeof(T));
    }

    public virtual async Task Actualizar(T registro)
    {
        Tabla.Update(registro);
        await Contexto.SaveChangesAsync();
        _cache?.Invalidar(typeof(T));
    }

    public virtual async Task Eliminar(T registro)
    {
        Tabla.Remove(registro);
        await Contexto.SaveChangesAsync();
        _cache?.Invalidar(typeof(T));
    }
}
