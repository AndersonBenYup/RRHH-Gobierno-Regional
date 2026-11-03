using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class TipoComportamientoLogica : LogicaBase<TipoComportamiento>
{
    public TipoComportamientoLogica(IRepositorio<TipoComportamiento> repositorio) : base(repositorio) { }

    public Task<TipoComportamiento?> Obtener(int idTipoComportamiento) => Repositorio.Obtener(idTipoComportamiento);
}
