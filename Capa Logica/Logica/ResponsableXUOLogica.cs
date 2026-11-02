using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class ResponsableXUOLogica : LogicaBase<ResponsableXUO>
{
    public ResponsableXUOLogica(IRepositorio<ResponsableXUO> repositorio) : base(repositorio) { }

    public Task<ResponsableXUO?> Obtener(int IdResponsableParam) => Repositorio.Obtener(IdResponsableParam);
}
