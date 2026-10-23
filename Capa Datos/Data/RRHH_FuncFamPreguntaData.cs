using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class RRHH_FuncFamPreguntaData
{
    private readonly EvoRRLDbContext _contexto;

    public RRHH_FuncFamPreguntaData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<RRHH_FuncFamPregunta>> Listar() => await _contexto.Set<RRHH_FuncFamPregunta>().AsNoTracking().ToListAsync();

    public async Task<RRHH_FuncFamPregunta?> Obtener(byte IdPreguntaParam)
    {
        return await _contexto.Set<RRHH_FuncFamPregunta>().FindAsync(IdPreguntaParam);
    }
}
