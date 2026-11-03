using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class RRHH_FuncFamLogica : LogicaBase<RRHH_FuncFam>
{
    public RRHH_FuncFamLogica(IRepositorio<RRHH_FuncFam> repositorio) : base(repositorio) { }

    public Task<RRHH_FuncFam?> Obtener(int idEmpleado) => Repositorio.Obtener(idEmpleado);
}
