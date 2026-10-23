using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class RRHH_RelacPHData
{
    private readonly EvoRRLDbContext _contexto;

    public RRHH_RelacPHData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<RRHH_RelacPH>> Listar() => await _contexto.Set<RRHH_RelacPH>().ToListAsync();

    public async Task<RRHH_RelacPH?> Obtener(int idRelacPHParam)
    {
        return await _contexto.Set<RRHH_RelacPH>().FindAsync(idRelacPHParam);
    }

    public async Task Crear(RRHH_RelacPH registro)
    {
        _contexto.Set<RRHH_RelacPH>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(RRHH_RelacPH registro)
    {
        _contexto.Set<RRHH_RelacPH>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(RRHH_RelacPH registro)
    {
        _contexto.Set<RRHH_RelacPH>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
