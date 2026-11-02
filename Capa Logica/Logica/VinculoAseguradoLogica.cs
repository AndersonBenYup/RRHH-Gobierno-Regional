using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class VinculoAseguradoLogica : LogicaBase<VinculoAsegurado>
{
    public VinculoAseguradoLogica(IRepositorio<VinculoAsegurado> repositorio) : base(repositorio) { }

    public Task<VinculoAsegurado?> Obtener(int IdVinculoAseguradoParam) => Repositorio.Obtener(IdVinculoAseguradoParam);
}
