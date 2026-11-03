using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class Empleado_AreaLogica : LogicaBase<Empleado_Area>
{
    public Empleado_AreaLogica(IRepositorio<Empleado_Area> repositorio) : base(repositorio) { }

    public Task<Empleado_Area?> Obtener(int year, int idAreaOrganiz, int idEmpleado) => Repositorio.Obtener(year, idAreaOrganiz, idEmpleado);
}
