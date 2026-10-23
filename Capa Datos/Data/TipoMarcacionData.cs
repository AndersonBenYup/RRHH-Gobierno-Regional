using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class TipoMarcacionData
{
    private readonly EvoRRLDbContext _contexto;

    public TipoMarcacionData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<TipoMarcacion>> Listar() => await _contexto.Set<TipoMarcacion>().ToListAsync();

    public async Task<TipoMarcacion?> Obtener(string CodigoParam)
    {
        return await _contexto.Set<TipoMarcacion>().FindAsync(CodigoParam);
    }

    public async Task Crear(TipoMarcacion registro)
    {
        _contexto.Set<TipoMarcacion>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(TipoMarcacion registro)
    {
        _contexto.Set<TipoMarcacion>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(TipoMarcacion registro)
    {
        _contexto.Set<TipoMarcacion>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
