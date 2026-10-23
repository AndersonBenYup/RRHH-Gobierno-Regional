using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class RRHH_AseguradoData
{
    private readonly EvoRRLDbContext _contexto;

    public RRHH_AseguradoData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<RRHH_Asegurado>> Listar() => await _contexto.Set<RRHH_Asegurado>().ToListAsync();

    public async Task<RRHH_Asegurado?> Obtener(int idEmpleadoParam, int NroBeneficiarioParam)
    {
        return await _contexto.Set<RRHH_Asegurado>().FindAsync(idEmpleadoParam, NroBeneficiarioParam);
    }

    public async Task Crear(RRHH_Asegurado registro)
    {
        _contexto.Set<RRHH_Asegurado>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(RRHH_Asegurado registro)
    {
        _contexto.Set<RRHH_Asegurado>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(RRHH_Asegurado registro)
    {
        _contexto.Set<RRHH_Asegurado>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
