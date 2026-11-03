using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class LocalLogica : LogicaBase<Local>
{
    public LocalLogica(IRepositorio<Local> repositorio) : base(repositorio) { }

    public Task<Local?> Obtener(int idLocal) => Repositorio.Obtener(idLocal);
}
