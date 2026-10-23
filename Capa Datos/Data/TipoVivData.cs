using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class TipoVivData
{
    private readonly EvoRRLDbContext _contexto;

    public TipoVivData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<TipoViv>> Listar() => await _contexto.Set<TipoViv>().ToListAsync();

    public async Task<TipoViv?> Obtener(int IdTipoParam)
    {
        return await _contexto.Set<TipoViv>().FindAsync(IdTipoParam);
    }

    public async Task Crear(TipoViv registro)
    {
        _contexto.Set<TipoViv>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(TipoViv registro)
    {
        _contexto.Set<TipoViv>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(TipoViv registro)
    {
        _contexto.Set<TipoViv>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
