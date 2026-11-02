using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class ViviendaServicioLogica : LogicaBase<ViviendaServicio>
{
    public ViviendaServicioLogica(IRepositorio<ViviendaServicio> repositorio) : base(repositorio) { }

    public Task<ViviendaServicio?> Obtener(int IdViviendaParam, int IdServicioParam) => Repositorio.Obtener(IdViviendaParam, IdServicioParam);
}
