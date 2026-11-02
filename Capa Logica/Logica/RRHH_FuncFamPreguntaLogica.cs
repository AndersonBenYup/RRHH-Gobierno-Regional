using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class RRHH_FuncFamPreguntaLogica : LogicaBase<RRHH_FuncFamPregunta>
{
    public RRHH_FuncFamPreguntaLogica(IRepositorio<RRHH_FuncFamPregunta> repositorio) : base(repositorio) { }

    public Task<RRHH_FuncFamPregunta?> Obtener(byte IdPreguntaParam) => Repositorio.Obtener(IdPreguntaParam);
}
