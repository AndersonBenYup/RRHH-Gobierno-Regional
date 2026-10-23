using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class RRHH_DinamicaFamiliarData
{
    private readonly EvoRRLDbContext _contexto;

    public RRHH_DinamicaFamiliarData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<RRHH_DinamicaFamiliar>> Listar() => await _contexto.Set<RRHH_DinamicaFamiliar>().ToListAsync();

    public async Task<RRHH_DinamicaFamiliar?> Obtener(int idDinamicaParam)
    {
        return await _contexto.Set<RRHH_DinamicaFamiliar>().FindAsync(idDinamicaParam);
    }

    public async Task Crear(RRHH_DinamicaFamiliar registro)
    {
        _contexto.Set<RRHH_DinamicaFamiliar>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(RRHH_DinamicaFamiliar registro)
    {
        _contexto.Set<RRHH_DinamicaFamiliar>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(RRHH_DinamicaFamiliar registro)
    {
        _contexto.Set<RRHH_DinamicaFamiliar>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
