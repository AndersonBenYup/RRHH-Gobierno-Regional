using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class TipoResolucionData
{
    private readonly EvoRRLDbContext _contexto;

    public TipoResolucionData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<TipoResolucion>> Listar() => await _contexto.Set<TipoResolucion>().ToListAsync();

    public async Task<TipoResolucion?> Obtener(int idTipoResolucionParam)
    {
        return await _contexto.Set<TipoResolucion>().FindAsync(idTipoResolucionParam);
    }

    public async Task Crear(TipoResolucion registro)
    {
        _contexto.Set<TipoResolucion>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(TipoResolucion registro)
    {
        _contexto.Set<TipoResolucion>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(TipoResolucion registro)
    {
        _contexto.Set<TipoResolucion>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
