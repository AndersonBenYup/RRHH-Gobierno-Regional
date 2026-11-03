using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class EstudiosRealizadoLogica : LogicaBase<EstudiosRealizado>
{
    public EstudiosRealizadoLogica(IRepositorio<EstudiosRealizado> repositorio) : base(repositorio) { }

    public Task<EstudiosRealizado?> Obtener(int idTipoEstudios, int idEmpleado, int correlativo) => Repositorio.Obtener(idTipoEstudios, idEmpleado, correlativo);
}
