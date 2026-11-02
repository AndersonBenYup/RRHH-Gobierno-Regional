using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class RotacionLogica : LogicaBase<Rotacion>
{
    public RotacionLogica(IRepositorio<Rotacion> repositorio) : base(repositorio) { }

    public Task<Rotacion?> Obtener(int idEmpleadoParam, int idRotacionParam) => Repositorio.Obtener(idEmpleadoParam, idRotacionParam);
}
