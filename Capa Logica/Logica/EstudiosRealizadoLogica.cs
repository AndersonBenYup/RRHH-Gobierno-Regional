using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class EstudiosRealizadoLogica : LogicaBase<EstudiosRealizado>
{
    public EstudiosRealizadoLogica(IRepositorio<EstudiosRealizado> repositorio) : base(repositorio) { }

    public Task<EstudiosRealizado?> Obtener(int IdTipoEstudiosParam, int IdEmpleadoParam, int CorrelativoParam) => Repositorio.Obtener(IdTipoEstudiosParam, IdEmpleadoParam, CorrelativoParam);
}
