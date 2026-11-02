using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class RRHH_TipoFamiliaLogica : LogicaBase<RRHH_TipoFamilia>
{
    public RRHH_TipoFamiliaLogica(IRepositorio<RRHH_TipoFamilia> repositorio) : base(repositorio) { }

    public Task<RRHH_TipoFamilia?> Obtener(int idTipoFamParam) => Repositorio.Obtener(idTipoFamParam);
}
