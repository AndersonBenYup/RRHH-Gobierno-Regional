using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class ConservacionVivLogica : LogicaBase<ConservacionViv>
{
    public ConservacionVivLogica(IRepositorio<ConservacionViv> repositorio) : base(repositorio) { }

    public Task<ConservacionViv?> Obtener(int idConservacion) => Repositorio.Obtener(idConservacion);
}
