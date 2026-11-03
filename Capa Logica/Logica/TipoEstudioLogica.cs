using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class TipoEstudioLogica : LogicaBase<TipoEstudio>
{
    public TipoEstudioLogica(IRepositorio<TipoEstudio> repositorio) : base(repositorio) { }

    public Task<TipoEstudio?> Obtener(int idTipoEstudios) => Repositorio.Obtener(idTipoEstudios);
}
