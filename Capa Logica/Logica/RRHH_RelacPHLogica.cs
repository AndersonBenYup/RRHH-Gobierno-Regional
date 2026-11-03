using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class RRHH_RelacPHLogica : LogicaBase<RRHH_RelacPH>
{
    public RRHH_RelacPHLogica(IRepositorio<RRHH_RelacPH> repositorio) : base(repositorio) { }

    public Task<RRHH_RelacPH?> Obtener(int idRelacPH) => Repositorio.Obtener(idRelacPH);
}
