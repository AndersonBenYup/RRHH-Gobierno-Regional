using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class PeriodoLaboralData
{
    private readonly EvoRRLDbContext _contexto;

    public PeriodoLaboralData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<PeriodoLaboral>> Listar() => await _contexto.Set<PeriodoLaboral>().ToListAsync();

    public async Task<PeriodoLaboral?> Obtener(int IdEmpleadoParam, int NroPeriodoParam)
    {
        return await _contexto.Set<PeriodoLaboral>().FindAsync(IdEmpleadoParam, NroPeriodoParam);
    }

    public async Task Crear(PeriodoLaboral registro)
    {
        _contexto.Set<PeriodoLaboral>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(PeriodoLaboral registro)
    {
        _contexto.Set<PeriodoLaboral>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(PeriodoLaboral registro)
    {
        _contexto.Set<PeriodoLaboral>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
