using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class FamiliarDetalleLogica : LogicaBase<FamiliarDetalle>
{
    public FamiliarDetalleLogica(IRepositorio<FamiliarDetalle> repositorio) : base(repositorio) { }

    public Task<FamiliarDetalle?> Obtener(int idEmpleado, int idPersona) => Repositorio.Obtener(idEmpleado, idPersona);
}
