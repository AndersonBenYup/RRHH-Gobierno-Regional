using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class RRHH_TipoFamiliaData
{
    private readonly EvoRRLDbContext _contexto;

    public RRHH_TipoFamiliaData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<RRHH_TipoFamilia>> Listar() => await _contexto.Set<RRHH_TipoFamilia>().ToListAsync();

    public async Task<RRHH_TipoFamilia?> Obtener(int idTipoFamParam)
    {
        return await _contexto.Set<RRHH_TipoFamilia>().FindAsync(idTipoFamParam);
    }

    public async Task Crear(RRHH_TipoFamilia registro)
    {
        _contexto.Set<RRHH_TipoFamilia>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(RRHH_TipoFamilia registro)
    {
        _contexto.Set<RRHH_TipoFamilia>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(RRHH_TipoFamilia registro)
    {
        _contexto.Set<RRHH_TipoFamilia>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
