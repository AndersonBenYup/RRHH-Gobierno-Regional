using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class Rol_AccesoData
{
    private readonly EvoRRLDbContext _contexto;

    public Rol_AccesoData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<Rol_Acceso>> Listar() => await _contexto.Set<Rol_Acceso>().ToListAsync();

    public async Task<Rol_Acceso?> Obtener(int IdRolParam, string IdSistemaOpcionParam)
    {
        return await _contexto.Set<Rol_Acceso>().FindAsync(IdRolParam, IdSistemaOpcionParam);
    }

    public async Task Crear(Rol_Acceso registro)
    {
        _contexto.Set<Rol_Acceso>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(Rol_Acceso registro)
    {
        _contexto.Set<Rol_Acceso>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(Rol_Acceso registro)
    {
        _contexto.Set<Rol_Acceso>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
