using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class ResponsableXUOData
{
    private readonly EvoRRLDbContext _contexto;

    public ResponsableXUOData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<ResponsableXUO>> Listar() => await _contexto.Set<ResponsableXUO>().ToListAsync();

    public async Task<ResponsableXUO?> Obtener(int IdResponsableParam)
    {
        return await _contexto.Set<ResponsableXUO>().FindAsync(IdResponsableParam);
    }

    public async Task Crear(ResponsableXUO registro)
    {
        _contexto.Set<ResponsableXUO>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(ResponsableXUO registro)
    {
        _contexto.Set<ResponsableXUO>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(ResponsableXUO registro)
    {
        _contexto.Set<ResponsableXUO>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
