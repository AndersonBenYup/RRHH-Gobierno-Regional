using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class TipoPermisoLogica : LogicaBase<TipoPermiso>
{
    public TipoPermisoLogica(IRepositorio<TipoPermiso> repositorio) : base(repositorio) { }

    public Task<TipoPermiso?> Obtener(int idTipoPermisoParam) => Repositorio.Obtener(idTipoPermisoParam);
}
