using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class RRHH_SaludLogica : LogicaBase<RRHH_Salud>
{
    public RRHH_SaludLogica(IRepositorio<RRHH_Salud> repositorio) : base(repositorio) { }

    public Task<RRHH_Salud?> Obtener(int idSalud) => Repositorio.Obtener(idSalud);
}
