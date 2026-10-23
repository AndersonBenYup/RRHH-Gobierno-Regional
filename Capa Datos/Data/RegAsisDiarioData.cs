using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class RegAsisDiarioData
{
    private readonly EvoRRLDbContext _contexto;

    public RegAsisDiarioData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<RegAsisDiario>> Listar() => await _contexto.Set<RegAsisDiario>().ToListAsync();

    public async Task<RegAsisDiario?> Obtener(DateTime FechaParam, int IdEmpleadoParam)
    {
        return await _contexto.Set<RegAsisDiario>().FindAsync(FechaParam, IdEmpleadoParam);
    }

    public async Task Crear(RegAsisDiario registro)
    {
        _contexto.Set<RegAsisDiario>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(RegAsisDiario registro)
    {
        _contexto.Set<RegAsisDiario>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(RegAsisDiario registro)
    {
        _contexto.Set<RegAsisDiario>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
