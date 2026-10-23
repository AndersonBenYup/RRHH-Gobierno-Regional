using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class TipoComportamientoData
{
    private readonly EvoRRLDbContext _contexto;

    public TipoComportamientoData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<TipoComportamiento>> Listar() => await _contexto.Set<TipoComportamiento>().ToListAsync();

    public async Task<TipoComportamiento?> Obtener(int idTipoComportamientoParam)
    {
        return await _contexto.Set<TipoComportamiento>().FindAsync(idTipoComportamientoParam);
    }

    public async Task Crear(TipoComportamiento registro)
    {
        _contexto.Set<TipoComportamiento>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(TipoComportamiento registro)
    {
        _contexto.Set<TipoComportamiento>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(TipoComportamiento registro)
    {
        _contexto.Set<TipoComportamiento>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
