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

    public RepositorioBase(EvoRRLDbContext contexto) => Contexto = contexto;

    protected DbSet<T> Tabla => Contexto.Set<T>();

    public virtual async Task<List<T>> Listar() => await Tabla.ToListAsync();

    public virtual async Task<T?> Obtener(params object[] claves) => await Tabla.FindAsync(claves);

    public virtual async Task Crear(T registro)
    {
        Tabla.Add(registro);
        await Contexto.SaveChangesAsync();
    }

    public virtual async Task Actualizar(T registro)
    {
        Tabla.Update(registro);
        await Contexto.SaveChangesAsync();
    }

    public virtual async Task Eliminar(T registro)
    {
        Tabla.Remove(registro);
        await Contexto.SaveChangesAsync();
    }
}
