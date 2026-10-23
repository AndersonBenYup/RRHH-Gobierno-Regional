using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class TipoInstitucionData
{
    private readonly EvoRRLDbContext _contexto;

    public TipoInstitucionData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<TipoInstitucion>> Listar() => await _contexto.Set<TipoInstitucion>().ToListAsync();

    public async Task<TipoInstitucion?> Obtener(int IdTipoInstitucionParam)
    {
        return await _contexto.Set<TipoInstitucion>().FindAsync(IdTipoInstitucionParam);
    }

    public async Task Crear(TipoInstitucion registro)
    {
        _contexto.Set<TipoInstitucion>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(TipoInstitucion registro)
    {
        _contexto.Set<TipoInstitucion>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(TipoInstitucion registro)
    {
        _contexto.Set<TipoInstitucion>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
