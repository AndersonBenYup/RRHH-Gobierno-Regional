using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class RRHH_AseguradoLogica : LogicaBase<RRHH_Asegurado>
{
    public RRHH_AseguradoLogica(IRepositorio<RRHH_Asegurado> repositorio) : base(repositorio) { }

    public Task<RRHH_Asegurado?> Obtener(int idEmpleadoParam, int NroBeneficiarioParam) => Repositorio.Obtener(idEmpleadoParam, NroBeneficiarioParam);
}
