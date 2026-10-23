using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class TipoPermisoData
{
    private readonly EvoRRLDbContext _contexto;

    public TipoPermisoData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<TipoPermiso>> Listar() => await _contexto.Set<TipoPermiso>().ToListAsync();

    public async Task<TipoPermiso?> Obtener(int idTipoPermisoParam)
    {
        return await _contexto.Set<TipoPermiso>().FindAsync(idTipoPermisoParam);
    }

    public async Task Crear(TipoPermiso registro)
    {
        _contexto.Set<TipoPermiso>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(TipoPermiso registro)
    {
        _contexto.Set<TipoPermiso>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(TipoPermiso registro)
    {
        _contexto.Set<TipoPermiso>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
