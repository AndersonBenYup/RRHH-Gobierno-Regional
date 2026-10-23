using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class TipoTrabajadorData
{
    private readonly EvoRRLDbContext _contexto;

    public TipoTrabajadorData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<TipoTrabajador>> Listar() => await _contexto.Set<TipoTrabajador>().ToListAsync();

    public async Task<TipoTrabajador?> Obtener(int IdTipoTrabajadorParam)
    {
        return await _contexto.Set<TipoTrabajador>().FindAsync(IdTipoTrabajadorParam);
    }

    public async Task Crear(TipoTrabajador registro)
    {
        _contexto.Set<TipoTrabajador>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(TipoTrabajador registro)
    {
        _contexto.Set<TipoTrabajador>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(TipoTrabajador registro)
    {
        _contexto.Set<TipoTrabajador>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
