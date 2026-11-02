using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class RRHH_DinamicaFamiliarLogica : LogicaBase<RRHH_DinamicaFamiliar>
{
    public RRHH_DinamicaFamiliarLogica(IRepositorio<RRHH_DinamicaFamiliar> repositorio) : base(repositorio) { }

    public Task<RRHH_DinamicaFamiliar?> Obtener(int idDinamicaParam) => Repositorio.Obtener(idDinamicaParam);
}
