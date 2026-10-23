using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class EmpleadoResumenFamiliarData
{
    private readonly EvoRRLDbContext _contexto;

    public EmpleadoResumenFamiliarData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<EmpleadoResumenFamiliar>> Listar() => await _contexto.Set<EmpleadoResumenFamiliar>().AsNoTracking().ToListAsync();

    public async Task<EmpleadoResumenFamiliar?> Obtener(int IdEmpleadoParam)
    {
        return await _contexto.Set<EmpleadoResumenFamiliar>().FindAsync(IdEmpleadoParam);
    }
}
