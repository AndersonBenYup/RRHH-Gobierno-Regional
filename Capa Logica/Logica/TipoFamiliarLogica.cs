using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class TipoFamiliarLogica : LogicaBase<TipoFamiliar>
{
    public TipoFamiliarLogica(IRepositorio<TipoFamiliar> repositorio) : base(repositorio) { }

    public Task<TipoFamiliar?> Obtener(int idTipoFam) => Repositorio.Obtener(idTipoFam);
}
