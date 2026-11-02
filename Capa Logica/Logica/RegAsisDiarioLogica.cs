using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class RegAsisDiarioLogica : LogicaBase<RegAsisDiario>
{
    public RegAsisDiarioLogica(IRepositorio<RegAsisDiario> repositorio) : base(repositorio) { }

    public Task<RegAsisDiario?> Obtener(DateTime FechaParam, int IdEmpleadoParam) => Repositorio.Obtener(FechaParam, IdEmpleadoParam);
}
