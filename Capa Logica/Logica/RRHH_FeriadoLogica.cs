using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class RRHH_FeriadoLogica : LogicaBase<RRHH_Feriado>
{
    public RRHH_FeriadoLogica(IRepositorio<RRHH_Feriado> repositorio) : base(repositorio) { }

    public Task<RRHH_Feriado?> Obtener(int idFeriadoParam) => Repositorio.Obtener(idFeriadoParam);
}
