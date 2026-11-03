using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class TitulosEmpleadoLogica : LogicaBase<TitulosEmpleado>
{
    public TitulosEmpleadoLogica(IRepositorio<TitulosEmpleado> repositorio) : base(repositorio) { }

    public Task<TitulosEmpleado?> Obtener(int idTitulo) => Repositorio.Obtener(idTitulo);
}
