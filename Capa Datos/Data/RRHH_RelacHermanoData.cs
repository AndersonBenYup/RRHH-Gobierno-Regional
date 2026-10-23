using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class RRHH_RelacHermanoData
{
    private readonly EvoRRLDbContext _contexto;

    public RRHH_RelacHermanoData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<RRHH_RelacHermano>> Listar() => await _contexto.Set<RRHH_RelacHermano>().ToListAsync();

    public async Task<RRHH_RelacHermano?> Obtener(int idRelacHermanoParam)
    {
        return await _contexto.Set<RRHH_RelacHermano>().FindAsync(idRelacHermanoParam);
    }

    public async Task Crear(RRHH_RelacHermano registro)
    {
        _contexto.Set<RRHH_RelacHermano>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(RRHH_RelacHermano registro)
    {
        _contexto.Set<RRHH_RelacHermano>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(RRHH_RelacHermano registro)
    {
        _contexto.Set<RRHH_RelacHermano>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
