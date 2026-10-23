using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class ResolucionData
{
    private readonly EvoRRLDbContext _contexto;

    public ResolucionData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<Resolucion>> Listar() => await _contexto.Set<Resolucion>().ToListAsync();

    public async Task<Resolucion?> Obtener(int idEmpleadoParam, int idResolucionParam)
    {
        return await _contexto.Set<Resolucion>().FindAsync(idEmpleadoParam, idResolucionParam);
    }

    public async Task Crear(Resolucion registro)
    {
        _contexto.Set<Resolucion>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(Resolucion registro)
    {
        _contexto.Set<Resolucion>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(Resolucion registro)
    {
        _contexto.Set<Resolucion>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
