using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class FamiliarLogica : LogicaBase<Familiar>
{
    public FamiliarLogica(IRepositorio<Familiar> repositorio) : base(repositorio) { }

    public Task<Familiar?> Obtener(int idEmpleado, int idPersona) => Repositorio.Obtener(idEmpleado, idPersona);
}
