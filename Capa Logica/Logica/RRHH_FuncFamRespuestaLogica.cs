using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class RRHH_FuncFamRespuestaLogica : LogicaBase<RRHH_FuncFamRespuesta>
{
    public RRHH_FuncFamRespuestaLogica(IRepositorio<RRHH_FuncFamRespuesta> repositorio) : base(repositorio) { }

    public Task<RRHH_FuncFamRespuesta?> Obtener(int IdEmpleadoParam, byte IdPreguntaParam) => Repositorio.Obtener(IdEmpleadoParam, IdPreguntaParam);
}
