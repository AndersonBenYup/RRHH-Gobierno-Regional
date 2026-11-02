using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class TipoInstitucionLogica : LogicaBase<TipoInstitucion>
{
    public TipoInstitucionLogica(IRepositorio<TipoInstitucion> repositorio) : base(repositorio) { }

    public Task<TipoInstitucion?> Obtener(int IdTipoInstitucionParam) => Repositorio.Obtener(IdTipoInstitucionParam);
}
