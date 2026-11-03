using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class EmpleadoAreaActualLogica : LogicaBase<EmpleadoAreaActual>
{
    public EmpleadoAreaActualLogica(IRepositorio<EmpleadoAreaActual> repositorio) : base(repositorio) { }

    public Task<EmpleadoAreaActual?> Obtener(int idEmpleado) => Repositorio.Obtener(idEmpleado);
}
