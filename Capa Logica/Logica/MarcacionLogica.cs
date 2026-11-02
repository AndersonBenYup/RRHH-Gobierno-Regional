using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class MarcacionLogica : LogicaBase<Marcacion>
{
    public MarcacionLogica(IRepositorio<Marcacion> repositorio) : base(repositorio) { }

    public Task<Marcacion?> Obtener(DateTime FechaParam, int IdEmpleadoParam) => Repositorio.Obtener(FechaParam, IdEmpleadoParam);
}
