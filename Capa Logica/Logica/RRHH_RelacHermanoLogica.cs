using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class RRHH_RelacHermanoLogica : LogicaBase<RRHH_RelacHermano>
{
    public RRHH_RelacHermanoLogica(IRepositorio<RRHH_RelacHermano> repositorio) : base(repositorio) { }

    public Task<RRHH_RelacHermano?> Obtener(int idRelacHermano) => Repositorio.Obtener(idRelacHermano);
}
