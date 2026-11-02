using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class RRHH_RelacParejaLogica : LogicaBase<RRHH_RelacPareja>
{
    public RRHH_RelacParejaLogica(IRepositorio<RRHH_RelacPareja> repositorio) : base(repositorio) { }

    public Task<RRHH_RelacPareja?> Obtener(int idRelacParejaParam) => Repositorio.Obtener(idRelacParejaParam);
}
