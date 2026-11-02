using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class RolLogica : LogicaBase<Rol>
{
    public RolLogica(IRepositorio<Rol> repositorio) : base(repositorio) { }

    public Task<Rol?> Obtener(int IdRolParam) => Repositorio.Obtener(IdRolParam);
}
