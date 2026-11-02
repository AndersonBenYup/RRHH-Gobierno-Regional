using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class TipoDocIDLogica : LogicaBase<TipoDocID>
{
    public TipoDocIDLogica(IRepositorio<TipoDocID> repositorio) : base(repositorio) { }

    public Task<TipoDocID?> Obtener(int TipoDocIDParam) => Repositorio.Obtener(TipoDocIDParam);
}
