using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class TipoDocIDData
{
    private readonly EvoRRLDbContext _contexto;

    public TipoDocIDData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<TipoDocID>> Listar() => await _contexto.Set<TipoDocID>().ToListAsync();

    public async Task<TipoDocID?> Obtener(int TipoDocIDParam)
    {
        return await _contexto.Set<TipoDocID>().FindAsync(TipoDocIDParam);
    }

    public async Task Crear(TipoDocID registro)
    {
        _contexto.Set<TipoDocID>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(TipoDocID registro)
    {
        _contexto.Set<TipoDocID>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(TipoDocID registro)
    {
        _contexto.Set<TipoDocID>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
