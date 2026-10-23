using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class EmpleadoAreaActualData
{
    private readonly EvoRRLDbContext _contexto;

    public EmpleadoAreaActualData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<EmpleadoAreaActual>> Listar() => await _contexto.Set<EmpleadoAreaActual>().AsNoTracking().ToListAsync();

    public async Task<EmpleadoAreaActual?> Obtener(int IdEmpleadoParam)
    {
        return await _contexto.Set<EmpleadoAreaActual>().FindAsync(IdEmpleadoParam);
    }
}
