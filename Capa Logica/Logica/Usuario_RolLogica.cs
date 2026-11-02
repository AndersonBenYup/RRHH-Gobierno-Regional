using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class Usuario_RolLogica : LogicaBase<Usuario_Rol>
{
    public Usuario_RolLogica(IRepositorio<Usuario_Rol> repositorio) : base(repositorio) { }

    public Task<Usuario_Rol?> Obtener(int IdUsuarioParam, int IdRolParam) => Repositorio.Obtener(IdUsuarioParam, IdRolParam);
}
