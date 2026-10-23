using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class TipoEstudioData
{
    private readonly EvoRRLDbContext _contexto;

    public TipoEstudioData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<TipoEstudio>> Listar() => await _contexto.Set<TipoEstudio>().ToListAsync();

    public async Task<TipoEstudio?> Obtener(int IdTipoEstudiosParam)
    {
        return await _contexto.Set<TipoEstudio>().FindAsync(IdTipoEstudiosParam);
    }

    public async Task Crear(TipoEstudio registro)
    {
        _contexto.Set<TipoEstudio>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(TipoEstudio registro)
    {
        _contexto.Set<TipoEstudio>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(TipoEstudio registro)
    {
        _contexto.Set<TipoEstudio>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
