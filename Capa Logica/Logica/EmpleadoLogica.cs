using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class EmpleadoLogica : LogicaBase<Empleado>
{
    public EmpleadoLogica(IRepositorio<Empleado> repositorio) : base(repositorio) { }

    public Task<Empleado?> Obtener(int IdEmpleadoParam) => Repositorio.Obtener(IdEmpleadoParam);
}
