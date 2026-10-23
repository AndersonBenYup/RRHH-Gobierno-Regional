using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class RolData
{
    private readonly EvoRRLDbContext _contexto;

    public RolData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<Rol>> Listar() => await _contexto.Set<Rol>().ToListAsync();

    public async Task<Rol?> Obtener(int IdRolParam)
    {
        return await _contexto.Set<Rol>().FindAsync(IdRolParam);
    }

    public async Task Crear(Rol registro)
    {
        _contexto.Set<Rol>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(Rol registro)
    {
        _contexto.Set<Rol>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(Rol registro)
    {
        _contexto.Set<Rol>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
