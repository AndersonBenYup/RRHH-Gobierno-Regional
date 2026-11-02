using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class EmpleadoResumenFamiliarLogica : LogicaBase<EmpleadoResumenFamiliar>
{
    public EmpleadoResumenFamiliarLogica(IRepositorio<EmpleadoResumenFamiliar> repositorio) : base(repositorio) { }

    public Task<EmpleadoResumenFamiliar?> Obtener(int IdEmpleadoParam) => Repositorio.Obtener(IdEmpleadoParam);
}
