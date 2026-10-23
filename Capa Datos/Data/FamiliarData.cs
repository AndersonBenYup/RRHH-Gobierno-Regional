using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class FamiliarData
{
    private readonly EvoRRLDbContext _contexto;

    public FamiliarData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<Familiar>> Listar() => await _contexto.Set<Familiar>().ToListAsync();

    public async Task<Familiar?> Obtener(int IdEmpleadoParam, int IdPersonaParam)
    {
        return await _contexto.Set<Familiar>().FindAsync(IdEmpleadoParam, IdPersonaParam);
    }

    public async Task Crear(Familiar registro)
    {
        _contexto.Set<Familiar>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(Familiar registro)
    {
        _contexto.Set<Familiar>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(Familiar registro)
    {
        _contexto.Set<Familiar>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
