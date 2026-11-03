using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class ViviendaLogica : LogicaBase<Vivienda>
{
    public ViviendaLogica(IRepositorio<Vivienda> repositorio) : base(repositorio) { }

    public Task<Vivienda?> Obtener(int idVivienda) => Repositorio.Obtener(idVivienda);
}
