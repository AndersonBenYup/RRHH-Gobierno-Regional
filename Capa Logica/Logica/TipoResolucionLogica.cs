using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class TipoResolucionLogica : LogicaBase<TipoResolucion>
{
    public TipoResolucionLogica(IRepositorio<TipoResolucion> repositorio) : base(repositorio) { }

    public Task<TipoResolucion?> Obtener(int idTipoResolucionParam) => Repositorio.Obtener(idTipoResolucionParam);
}
