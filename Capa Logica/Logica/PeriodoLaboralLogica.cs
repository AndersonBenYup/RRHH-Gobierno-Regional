using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class PeriodoLaboralLogica : LogicaBase<PeriodoLaboral>
{
    public PeriodoLaboralLogica(IRepositorio<PeriodoLaboral> repositorio) : base(repositorio) { }

    public Task<PeriodoLaboral?> Obtener(int idEmpleado, int nroPeriodo) => Repositorio.Obtener(idEmpleado, nroPeriodo);
}
