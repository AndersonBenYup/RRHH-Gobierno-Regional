using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class RegimenPensionData
{
    private readonly EvoRRLDbContext _contexto;

    public RegimenPensionData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<RegimenPension>> Listar() => await _contexto.Set<RegimenPension>().ToListAsync();

    public async Task<RegimenPension?> Obtener(int IdRegimenPenParam)
    {
        return await _contexto.Set<RegimenPension>().FindAsync(IdRegimenPenParam);
    }

    public async Task Crear(RegimenPension registro)
    {
        _contexto.Set<RegimenPension>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(RegimenPension registro)
    {
        _contexto.Set<RegimenPension>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(RegimenPension registro)
    {
        _contexto.Set<RegimenPension>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
