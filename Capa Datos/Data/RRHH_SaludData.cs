using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class RRHH_SaludData
{
    private readonly EvoRRLDbContext _contexto;

    public RRHH_SaludData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<RRHH_Salud>> Listar() => await _contexto.Set<RRHH_Salud>().ToListAsync();

    public async Task<RRHH_Salud?> Obtener(int idSaludParam)
    {
        return await _contexto.Set<RRHH_Salud>().FindAsync(idSaludParam);
    }

    public async Task Crear(RRHH_Salud registro)
    {
        _contexto.Set<RRHH_Salud>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(RRHH_Salud registro)
    {
        _contexto.Set<RRHH_Salud>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(RRHH_Salud registro)
    {
        _contexto.Set<RRHH_Salud>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
