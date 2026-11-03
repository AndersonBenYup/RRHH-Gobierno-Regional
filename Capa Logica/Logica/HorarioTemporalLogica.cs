using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class HorarioTemporalLogica : LogicaBase<HorarioTemporal>
{
    public HorarioTemporalLogica(IRepositorio<HorarioTemporal> repositorio) : base(repositorio) { }

    public Task<HorarioTemporal?> Obtener(int idEmpleado, int n) => Repositorio.Obtener(idEmpleado, n);
}
