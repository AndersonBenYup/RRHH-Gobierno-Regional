using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class EstadoAsistenciaData
{
    private readonly EvoRRLDbContext _contexto;

    public EstadoAsistenciaData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<EstadoAsistencia>> Listar() => await _contexto.Set<EstadoAsistencia>().ToListAsync();

    public async Task<EstadoAsistencia?> Obtener(string CodigoParam)
    {
        return await _contexto.Set<EstadoAsistencia>().FindAsync(CodigoParam);
    }

    public async Task Crear(EstadoAsistencia registro)
    {
        _contexto.Set<EstadoAsistencia>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(EstadoAsistencia registro)
    {
        _contexto.Set<EstadoAsistencia>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(EstadoAsistencia registro)
    {
        _contexto.Set<EstadoAsistencia>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
