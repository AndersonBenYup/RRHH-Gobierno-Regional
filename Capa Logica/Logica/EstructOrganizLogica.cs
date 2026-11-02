using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class EstructOrganizLogica : LogicaBase<EstructOrganiz>
{
    public EstructOrganizLogica(IRepositorio<EstructOrganiz> repositorio) : base(repositorio) { }

    public Task<EstructOrganiz?> Obtener(int YearParam, int idAreaOrganizParam) => Repositorio.Obtener(YearParam, idAreaOrganizParam);
}
