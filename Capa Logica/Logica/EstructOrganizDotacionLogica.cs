using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class EstructOrganizDotacionLogica : LogicaBase<EstructOrganizDotacion>
{
    public EstructOrganizDotacionLogica(IRepositorio<EstructOrganizDotacion> repositorio) : base(repositorio) { }

    public Task<EstructOrganizDotacion?> Obtener(int year, int idAreaOrganiz) => Repositorio.Obtener(year, idAreaOrganiz);
}
