using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class Usuario_RolData
{
    private readonly EvoRRLDbContext _contexto;

    public Usuario_RolData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<Usuario_Rol>> Listar() => await _contexto.Set<Usuario_Rol>().ToListAsync();

    public async Task<Usuario_Rol?> Obtener(int IdUsuarioParam, int IdRolParam)
    {
        return await _contexto.Set<Usuario_Rol>().FindAsync(IdUsuarioParam, IdRolParam);
    }

    public async Task Crear(Usuario_Rol registro)
    {
        _contexto.Set<Usuario_Rol>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(Usuario_Rol registro)
    {
        _contexto.Set<Usuario_Rol>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(Usuario_Rol registro)
    {
        _contexto.Set<Usuario_Rol>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
