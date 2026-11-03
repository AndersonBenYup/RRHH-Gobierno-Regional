using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class RRHH_AcudeEnfermLogica : LogicaBase<RRHH_AcudeEnferm>
{
    public RRHH_AcudeEnfermLogica(IRepositorio<RRHH_AcudeEnferm> repositorio) : base(repositorio) { }

    public Task<RRHH_AcudeEnferm?> Obtener(int idAcudeEnferm) => Repositorio.Obtener(idAcudeEnferm);
}
