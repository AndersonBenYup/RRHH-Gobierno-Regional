using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class AsistenciaData
{
    private readonly EvoRRLDbContext _contexto;

    public AsistenciaData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<Asistencia>> Listar() => await _contexto.Set<Asistencia>().ToListAsync();

    public async Task<int> ContarEntreFechas(DateTime desde, DateTime hasta)
        => await _contexto.Set<Asistencia>().CountAsync(a => a.Fecha.Date >= desde.Date && a.Fecha.Date <= hasta.Date);

    public async Task<Asistencia?> Obtener(DateTime FechaParam, int IdEmpleadoParam)
    {
        return await _contexto.Set<Asistencia>().FindAsync(FechaParam, IdEmpleadoParam);
    }

    public async Task Crear(Asistencia registro)
    {
        _contexto.Set<Asistencia>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(Asistencia registro)
    {
        _contexto.Set<Asistencia>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(Asistencia registro)
    {
        _contexto.Set<Asistencia>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
