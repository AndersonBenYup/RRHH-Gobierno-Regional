using Capa_Entidades;
using Capa_Entidades.Contratos;
using Microsoft.EntityFrameworkCore;

namespace Capa_Datos.Repositorios;

/// <summary>SC-005: los filtros y la paginación de permisos se ejecutan en SQL Server, no en memoria.</summary>
public class PermisoRepositorio : RepositorioBase<Permiso>, IPermisoRepositorio
{
    public PermisoRepositorio(EvoRRLDbContext contexto) : base(contexto) { }

    public async Task<List<Permiso>> ListarPorEmpleado(int idEmpleado)
        => await Tabla.AsNoTracking().Where(p => p.IdEmpleado == idEmpleado).ToListAsync();

    public async Task<Pagina<Permiso>> ListarPaginado(int? idEmpleado, int pagina, int tamanoPagina)
    {
        tamanoPagina = Math.Clamp(tamanoPagina, 1, 500);
        var consulta = Tabla.AsNoTracking().Where(p => idEmpleado == null || p.IdEmpleado == idEmpleado);

        var total = await consulta.CountAsync();
        var totalPaginas = Math.Max(1, (int)Math.Ceiling(total / (double)tamanoPagina));
        pagina = Math.Clamp(pagina, 1, totalPaginas);

        var registros = await consulta
            .OrderByDescending(p => p.FechaInicio)
            .ThenByDescending(p => p.NPermiso)
            .ThenBy(p => p.IdEmpleado)
            .Skip((pagina - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .ToListAsync();

        return new Pagina<Permiso>(registros, total, pagina, tamanoPagina);
    }

    public async Task<int> SiguienteNumero(int idEmpleado)
        => (await Tabla.Where(p => p.IdEmpleado == idEmpleado).MaxAsync(p => (int?)p.NPermiso) ?? 0) + 1;

    public async Task<List<Permiso>> ListarPorMotivosEntreFechas(IReadOnlyCollection<int> motivos, DateTime desde, DateTime hasta)
        => motivos.Count == 0 ? [] : await ConsultaPorMotivosEntreFechas(Tabla.AsNoTracking(), motivos, desde, hasta).ToListAsync();

    public async Task<List<Permiso>> ListarPorMotivos(IReadOnlyCollection<int> motivos, int? idEmpleado)
        => motivos.Count == 0 ? [] : await ConsultaPorMotivos(Tabla.AsNoTracking(), motivos, idEmpleado).ToListAsync();

    /// <summary>Consulta de <see cref="ListarPorMotivosEntreFechas"/>; pública para comprobar que EF la traduce a SQL.</summary>
    public static IQueryable<Permiso> ConsultaPorMotivosEntreFechas(IQueryable<Permiso> permisos,
        IReadOnlyCollection<int> motivos, DateTime desde, DateTime hasta)
    {
        var lista = motivos.ToList();
        var inicio = desde.Date;
        var fin = hasta.Date;
        return permisos
            .Where(p => p.idMotivo != null && lista.Contains(p.idMotivo.Value)
                && p.FechaInicio != null && p.FechaInicio <= fin
                && (p.FechaFin ?? p.FechaInicio) >= inicio)
            .OrderBy(p => p.FechaInicio).ThenBy(p => p.IdEmpleado);
    }

    /// <summary>Consulta de <see cref="ListarPorMotivos"/>; pública para comprobar que EF la traduce a SQL.</summary>
    public static IQueryable<Permiso> ConsultaPorMotivos(IQueryable<Permiso> permisos,
        IReadOnlyCollection<int> motivos, int? idEmpleado)
    {
        var lista = motivos.ToList();
        return permisos
            .Where(p => p.idMotivo != null && lista.Contains(p.idMotivo.Value)
                && (idEmpleado == null || p.IdEmpleado == idEmpleado))
            .OrderBy(p => p.IdEmpleado).ThenBy(p => p.FechaInicio);
    }
}
