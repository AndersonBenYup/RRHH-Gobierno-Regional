using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class RRHH_FuncFamRespuestaData
{
    private readonly EvoRRLDbContext _contexto;

    public RRHH_FuncFamRespuestaData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<RRHH_FuncFamRespuesta>> Listar() => await _contexto.Set<RRHH_FuncFamRespuesta>().ToListAsync();

    public async Task<RRHH_FuncFamRespuesta?> Obtener(int IdEmpleadoParam, byte IdPreguntaParam)
    {
        return await _contexto.Set<RRHH_FuncFamRespuesta>().FindAsync(IdEmpleadoParam, IdPreguntaParam);
    }

    public async Task Crear(RRHH_FuncFamRespuesta registro)
    {
        _contexto.Set<RRHH_FuncFamRespuesta>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(RRHH_FuncFamRespuesta registro)
    {
        _contexto.Set<RRHH_FuncFamRespuesta>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(RRHH_FuncFamRespuesta registro)
    {
        _contexto.Set<RRHH_FuncFamRespuesta>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
