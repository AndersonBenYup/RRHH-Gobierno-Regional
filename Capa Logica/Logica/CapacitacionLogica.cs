using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class CapacitacionLogica : LogicaBase<Capacitacion>
{
    public CapacitacionLogica(IRepositorio<Capacitacion> repositorio) : base(repositorio) { }

    public Task<Capacitacion?> Obtener(int IdCapacitacionParam) => Repositorio.Obtener(IdCapacitacionParam);
}
