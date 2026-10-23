using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class EstudiosRealizadoData
{
    private readonly EvoRRLDbContext _contexto;

    public EstudiosRealizadoData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<EstudiosRealizado>> Listar() => await _contexto.Set<EstudiosRealizado>().ToListAsync();

    public async Task<EstudiosRealizado?> Obtener(int IdTipoEstudiosParam, int IdEmpleadoParam, int CorrelativoParam)
    {
        return await _contexto.Set<EstudiosRealizado>().FindAsync(IdTipoEstudiosParam, IdEmpleadoParam, CorrelativoParam);
    }

    public async Task Crear(EstudiosRealizado registro)
    {
        _contexto.Set<EstudiosRealizado>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(EstudiosRealizado registro)
    {
        _contexto.Set<EstudiosRealizado>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(EstudiosRealizado registro)
    {
        _contexto.Set<EstudiosRealizado>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
