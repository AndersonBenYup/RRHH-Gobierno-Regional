using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class TipoMarcacionLogica : LogicaBase<TipoMarcacion>
{
    public TipoMarcacionLogica(IRepositorio<TipoMarcacion> repositorio) : base(repositorio) { }

    public Task<TipoMarcacion?> Obtener(string CodigoParam) => Repositorio.Obtener(CodigoParam);
}
