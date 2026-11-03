using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class AFPLogica : LogicaBase<AFP>
{
    public AFPLogica(IRepositorio<AFP> repositorio) : base(repositorio) { }

    public Task<AFP?> Obtener(int idAFP) => Repositorio.Obtener(idAFP);
}
