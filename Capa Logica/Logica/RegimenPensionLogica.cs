using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class RegimenPensionLogica : LogicaBase<RegimenPension>
{
    public RegimenPensionLogica(IRepositorio<RegimenPension> repositorio) : base(repositorio) { }

    public Task<RegimenPension?> Obtener(int idRegimenPen) => Repositorio.Obtener(idRegimenPen);
}
