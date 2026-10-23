using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class AFPData
{
    private readonly EvoRRLDbContext _contexto;

    public AFPData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<AFP>> Listar() => await _contexto.Set<AFP>().ToListAsync();

    public async Task<AFP?> Obtener(int IdAFPParam)
    {
        return await _contexto.Set<AFP>().FindAsync(IdAFPParam);
    }

    public async Task Crear(AFP registro)
    {
        _contexto.Set<AFP>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(AFP registro)
    {
        _contexto.Set<AFP>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(AFP registro)
    {
        _contexto.Set<AFP>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
