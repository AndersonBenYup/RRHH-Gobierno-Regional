using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class ExcretasVivLogica : LogicaBase<ExcretasViv>
{
    public ExcretasVivLogica(IRepositorio<ExcretasViv> repositorio) : base(repositorio) { }

    public Task<ExcretasViv?> Obtener(int IdExcretasParam) => Repositorio.Obtener(IdExcretasParam);
}
