using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class TipoTrabajadorLogica : LogicaBase<TipoTrabajador>
{
    public TipoTrabajadorLogica(IRepositorio<TipoTrabajador> repositorio) : base(repositorio) { }

    public Task<TipoTrabajador?> Obtener(int idTipoTrabajador) => Repositorio.Obtener(idTipoTrabajador);
}
