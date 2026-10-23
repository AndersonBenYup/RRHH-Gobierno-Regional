using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class TipoSangreData
{
    private readonly EvoRRLDbContext _contexto;

    public TipoSangreData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<TipoSangre>> Listar() => await _contexto.Set<TipoSangre>().ToListAsync();

    public async Task<TipoSangre?> Obtener(int IdTipoSangreParam)
    {
        return await _contexto.Set<TipoSangre>().FindAsync(IdTipoSangreParam);
    }

    public async Task Crear(TipoSangre registro)
    {
        _contexto.Set<TipoSangre>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(TipoSangre registro)
    {
        _contexto.Set<TipoSangre>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(TipoSangre registro)
    {
        _contexto.Set<TipoSangre>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
