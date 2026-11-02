using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class ProvinciaLogica : LogicaBase<Provincia>
{
    public ProvinciaLogica(IRepositorio<Provincia> repositorio) : base(repositorio) { }

    public Task<Provincia?> Obtener(string CodDepartamentoParam, string CodProvinciaParam) => Repositorio.Obtener(CodDepartamentoParam, CodProvinciaParam);
}
