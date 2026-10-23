using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class RRHH_AcudeEnfermData
{
    private readonly EvoRRLDbContext _contexto;

    public RRHH_AcudeEnfermData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<RRHH_AcudeEnferm>> Listar() => await _contexto.Set<RRHH_AcudeEnferm>().ToListAsync();

    public async Task<RRHH_AcudeEnferm?> Obtener(int idAcudeEnfermParam)
    {
        return await _contexto.Set<RRHH_AcudeEnferm>().FindAsync(idAcudeEnfermParam);
    }

    public async Task Crear(RRHH_AcudeEnferm registro)
    {
        _contexto.Set<RRHH_AcudeEnferm>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(RRHH_AcudeEnferm registro)
    {
        _contexto.Set<RRHH_AcudeEnferm>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(RRHH_AcudeEnferm registro)
    {
        _contexto.Set<RRHH_AcudeEnferm>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
