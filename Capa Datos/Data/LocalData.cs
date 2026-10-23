using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class LocalData
{
    private readonly EvoRRLDbContext _contexto;

    public LocalData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<Local>> Listar() => await _contexto.Set<Local>().ToListAsync();

    public async Task<Local?> Obtener(int idLocalParam)
    {
        return await _contexto.Set<Local>().FindAsync(idLocalParam);
    }

    public async Task Crear(Local registro)
    {
        _contexto.Set<Local>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(Local registro)
    {
        _contexto.Set<Local>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(Local registro)
    {
        _contexto.Set<Local>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
