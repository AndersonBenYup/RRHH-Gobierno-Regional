using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class RRHH_FeriadoData
{
    private readonly EvoRRLDbContext _contexto;

    public RRHH_FeriadoData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<RRHH_Feriado>> Listar() => await _contexto.Set<RRHH_Feriado>().ToListAsync();

    public async Task<RRHH_Feriado?> Obtener(int idFeriadoParam)
    {
        return await _contexto.Set<RRHH_Feriado>().FindAsync(idFeriadoParam);
    }

    public async Task Crear(RRHH_Feriado registro)
    {
        _contexto.Set<RRHH_Feriado>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(RRHH_Feriado registro)
    {
        _contexto.Set<RRHH_Feriado>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(RRHH_Feriado registro)
    {
        _contexto.Set<RRHH_Feriado>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
