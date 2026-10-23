using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class SistemaOpcionData
{
    private readonly EvoRRLDbContext _contexto;

    public SistemaOpcionData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<SistemaOpcion>> Listar() => await _contexto.Set<SistemaOpcion>().ToListAsync();

    public async Task<SistemaOpcion?> Obtener(string IdSistemaOpcionParam)
    {
        return await _contexto.Set<SistemaOpcion>().FindAsync(IdSistemaOpcionParam);
    }

    public async Task Crear(SistemaOpcion registro)
    {
        _contexto.Set<SistemaOpcion>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(SistemaOpcion registro)
    {
        _contexto.Set<SistemaOpcion>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(SistemaOpcion registro)
    {
        _contexto.Set<SistemaOpcion>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
