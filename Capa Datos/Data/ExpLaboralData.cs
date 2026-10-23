using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class ExpLaboralData
{
    private readonly EvoRRLDbContext _contexto;

    public ExpLaboralData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<ExpLaboral>> Listar() => await _contexto.Set<ExpLaboral>().ToListAsync();

    public async Task<ExpLaboral?> Obtener(int IdEmpleadoParam, int IdExpLabParam)
    {
        return await _contexto.Set<ExpLaboral>().FindAsync(IdEmpleadoParam, IdExpLabParam);
    }

    public async Task Crear(ExpLaboral registro)
    {
        _contexto.Set<ExpLaboral>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(ExpLaboral registro)
    {
        _contexto.Set<ExpLaboral>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(ExpLaboral registro)
    {
        _contexto.Set<ExpLaboral>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
