using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class SistemaOpcionLogica : LogicaBase<SistemaOpcion>
{
    public SistemaOpcionLogica(IRepositorio<SistemaOpcion> repositorio) : base(repositorio) { }

    public Task<SistemaOpcion?> Obtener(string idSistemaOpcion) => Repositorio.Obtener(idSistemaOpcion);
}
