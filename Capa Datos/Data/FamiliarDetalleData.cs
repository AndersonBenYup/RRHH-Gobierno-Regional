using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class FamiliarDetalleData
{
    private readonly EvoRRLDbContext _contexto;

    public FamiliarDetalleData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<FamiliarDetalle>> Listar() => await _contexto.Set<FamiliarDetalle>().AsNoTracking().ToListAsync();

    public async Task<FamiliarDetalle?> Obtener(int IdEmpleadoParam, int IdPersonaParam)
    {
        return await _contexto.Set<FamiliarDetalle>().FindAsync(IdEmpleadoParam, IdPersonaParam);
    }
}
