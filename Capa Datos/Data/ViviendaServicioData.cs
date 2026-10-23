using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class ViviendaServicioData
{
    private readonly EvoRRLDbContext _contexto;

    public ViviendaServicioData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<ViviendaServicio>> Listar() => await _contexto.Set<ViviendaServicio>().ToListAsync();

    public async Task<ViviendaServicio?> Obtener(int IdViviendaParam, int IdServicioParam)
    {
        return await _contexto.Set<ViviendaServicio>().FindAsync(IdViviendaParam, IdServicioParam);
    }

    public async Task Crear(ViviendaServicio registro)
    {
        _contexto.Set<ViviendaServicio>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(ViviendaServicio registro)
    {
        _contexto.Set<ViviendaServicio>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(ViviendaServicio registro)
    {
        _contexto.Set<ViviendaServicio>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
