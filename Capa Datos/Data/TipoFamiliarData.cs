using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class TipoFamiliarData
{
    private readonly EvoRRLDbContext _contexto;

    public TipoFamiliarData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<TipoFamiliar>> Listar() => await _contexto.Set<TipoFamiliar>().ToListAsync();

    public async Task<TipoFamiliar?> Obtener(int IdTipoFamParam)
    {
        return await _contexto.Set<TipoFamiliar>().FindAsync(IdTipoFamParam);
    }

    public async Task Crear(TipoFamiliar registro)
    {
        _contexto.Set<TipoFamiliar>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(TipoFamiliar registro)
    {
        _contexto.Set<TipoFamiliar>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(TipoFamiliar registro)
    {
        _contexto.Set<TipoFamiliar>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
