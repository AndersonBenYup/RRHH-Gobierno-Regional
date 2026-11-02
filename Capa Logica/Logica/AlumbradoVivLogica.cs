using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class AlumbradoVivLogica : LogicaBase<AlumbradoViv>
{
    public AlumbradoVivLogica(IRepositorio<AlumbradoViv> repositorio) : base(repositorio) { }

    public Task<AlumbradoViv?> Obtener(int IdAlumbradoParam) => Repositorio.Obtener(IdAlumbradoParam);
}
