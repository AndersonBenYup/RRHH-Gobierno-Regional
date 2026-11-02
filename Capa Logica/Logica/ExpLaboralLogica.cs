using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class ExpLaboralLogica : LogicaBase<ExpLaboral>
{
    public ExpLaboralLogica(IRepositorio<ExpLaboral> repositorio) : base(repositorio) { }

    public Task<ExpLaboral?> Obtener(int IdEmpleadoParam, int IdExpLabParam) => Repositorio.Obtener(IdEmpleadoParam, IdExpLabParam);
}
