using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class RRHH_AspSocioLogica : LogicaBase<RRHH_AspSocio>
{
    public RRHH_AspSocioLogica(IRepositorio<RRHH_AspSocio> repositorio) : base(repositorio) { }

    public Task<RRHH_AspSocio?> Obtener(int idAspSocioParam) => Repositorio.Obtener(idAspSocioParam);
}
