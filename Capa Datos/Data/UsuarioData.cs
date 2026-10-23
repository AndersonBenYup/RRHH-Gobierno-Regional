using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class UsuarioData
{
    private readonly EvoRRLDbContext _contexto;

    public UsuarioData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<Usuario>> Listar() => await _contexto.Set<Usuario>().ToListAsync();

    public async Task<Usuario?> Obtener(int IdUsuarioParam)
    {
        return await _contexto.Set<Usuario>().FindAsync(IdUsuarioParam);
    }

    public async Task<Usuario?> ObtenerPorLogin(string login) =>
        await _contexto.Set<Usuario>().FirstOrDefaultAsync(u => u.Login != null && u.Login.Trim() == login);

    public async Task<List<Rol>> ObtenerRoles(int idUsuario) =>
        await _contexto.Set<Usuario_Rol>()
            .Where(ur => ur.IdUsuario == idUsuario)
            .Join(_contexto.Set<Rol>(), ur => ur.IdRol, r => r.IdRol, (ur, r) => r)
            .ToListAsync();

    public async Task Crear(Usuario registro)
    {
        _contexto.Set<Usuario>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(Usuario registro)
    {
        _contexto.Set<Usuario>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(Usuario registro)
    {
        _contexto.Set<Usuario>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
