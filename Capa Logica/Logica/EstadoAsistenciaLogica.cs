using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class EstadoAsistenciaLogica : LogicaBase<EstadoAsistencia>
{
    public EstadoAsistenciaLogica(IRepositorio<EstadoAsistencia> repositorio) : base(repositorio) { }

    public Task<EstadoAsistencia?> Obtener(string codigo) => Repositorio.Obtener(codigo);
}
