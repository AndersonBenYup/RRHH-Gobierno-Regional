using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class TipoVivLogica : LogicaBase<TipoViv>
{
    public TipoVivLogica(IRepositorio<TipoViv> repositorio) : base(repositorio) { }

    public Task<TipoViv?> Obtener(int idTipo) => Repositorio.Obtener(idTipo);
}
