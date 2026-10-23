using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class HorarioTemporalData
{
    private readonly EvoRRLDbContext _contexto;

    public HorarioTemporalData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<HorarioTemporal>> Listar() => await _contexto.Set<HorarioTemporal>().ToListAsync();

    public async Task<HorarioTemporal?> Obtener(int idEmpleadoParam, int NParam)
    {
        return await _contexto.Set<HorarioTemporal>().FindAsync(idEmpleadoParam, NParam);
    }

    public async Task Crear(HorarioTemporal registro)
    {
        _contexto.Set<HorarioTemporal>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(HorarioTemporal registro)
    {
        _contexto.Set<HorarioTemporal>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(HorarioTemporal registro)
    {
        _contexto.Set<HorarioTemporal>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
