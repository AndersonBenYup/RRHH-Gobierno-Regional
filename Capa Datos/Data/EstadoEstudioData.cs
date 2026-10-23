using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class EstadoEstudioData
{
    private readonly EvoRRLDbContext _contexto;

    public EstadoEstudioData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<EstadoEstudio>> Listar() => await _contexto.Set<EstadoEstudio>().ToListAsync();

    public async Task<EstadoEstudio?> Obtener(int IdEstadoEstudioParam)
    {
        return await _contexto.Set<EstadoEstudio>().FindAsync(IdEstadoEstudioParam);
    }

    public async Task Crear(EstadoEstudio registro)
    {
        _contexto.Set<EstadoEstudio>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(EstadoEstudio registro)
    {
        _contexto.Set<EstadoEstudio>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(EstadoEstudio registro)
    {
        _contexto.Set<EstadoEstudio>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
