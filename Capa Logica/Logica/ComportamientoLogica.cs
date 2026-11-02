using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class ComportamientoLogica : LogicaBase<Comportamiento>
{
    public ComportamientoLogica(IRepositorio<Comportamiento> repositorio) : base(repositorio) { }

    public Task<Comportamiento?> Obtener(int idComportamientoParam) => Repositorio.Obtener(idComportamientoParam);
}
