using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class RRHH_RelacParejaData
{
    private readonly EvoRRLDbContext _contexto;

    public RRHH_RelacParejaData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<RRHH_RelacPareja>> Listar() => await _contexto.Set<RRHH_RelacPareja>().ToListAsync();

    public async Task<RRHH_RelacPareja?> Obtener(int idRelacParejaParam)
    {
        return await _contexto.Set<RRHH_RelacPareja>().FindAsync(idRelacParejaParam);
    }

    public async Task Crear(RRHH_RelacPareja registro)
    {
        _contexto.Set<RRHH_RelacPareja>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(RRHH_RelacPareja registro)
    {
        _contexto.Set<RRHH_RelacPareja>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(RRHH_RelacPareja registro)
    {
        _contexto.Set<RRHH_RelacPareja>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
