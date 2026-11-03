using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class UsuarioLogica : LogicaBase<Usuario>
{
    private readonly IUsuarioRepositorio _usuarios;

    public UsuarioLogica(IUsuarioRepositorio usuarios) : base(usuarios) => _usuarios = usuarios;

    public Task<Usuario?> Obtener(int idUsuario) => Repositorio.Obtener(idUsuario);
    public Task<Usuario?> ObtenerPorLogin(string login) => _usuarios.ObtenerPorLogin(login);
    public Task<List<Rol>> ObtenerRoles(int idUsuario) => _usuarios.ObtenerRoles(idUsuario);
}
