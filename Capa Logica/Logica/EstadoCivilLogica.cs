using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class EstadoCivilLogica : LogicaBase<EstadoCivil>
{
    public EstadoCivilLogica(IRepositorio<EstadoCivil> repositorio) : base(repositorio) { }

    public Task<EstadoCivil?> Obtener(int idEstadoCivil) => Repositorio.Obtener(idEstadoCivil);
}
