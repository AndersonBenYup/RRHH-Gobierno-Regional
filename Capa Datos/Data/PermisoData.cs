using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class PermisoData
{
    private readonly EvoRRLDbContext _contexto;

    public PermisoData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<Permiso>> Listar() => await _contexto.Set<Permiso>().ToListAsync();

    public async Task<Permiso?> Obtener(int IdEmpleadoParam, int NPermisoParam)
    {
        return await _contexto.Set<Permiso>().FindAsync(IdEmpleadoParam, NPermisoParam);
    }

    public async Task Crear(Permiso registro)
    {
        _contexto.Set<Permiso>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(Permiso registro)
    {
        _contexto.Set<Permiso>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(Permiso registro)
    {
        _contexto.Set<Permiso>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
