using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class EstadoEstudioLogica : LogicaBase<EstadoEstudio>
{
    public EstadoEstudioLogica(IRepositorio<EstadoEstudio> repositorio) : base(repositorio) { }

    public Task<EstadoEstudio?> Obtener(int idEstadoEstudio) => Repositorio.Obtener(idEstadoEstudio);
}
