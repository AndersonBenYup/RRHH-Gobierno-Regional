using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class RRHH_FuncFamPreguntaLogica
{
    private readonly RRHH_FuncFamPreguntaData _data;

    public RRHH_FuncFamPreguntaLogica(RRHH_FuncFamPreguntaData data) => _data = data;

    public async Task<List<RRHH_FuncFamPregunta>> Listar() => await _data.Listar();
    public async Task<RRHH_FuncFamPregunta?> Obtener(byte IdPreguntaParam) => await _data.Obtener(IdPreguntaParam);
}
