using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class UsuarioLogica
{
    private readonly UsuarioData _data;

    public UsuarioLogica(UsuarioData data) => _data = data;

    public async Task<List<Usuario>> Listar() => await _data.Listar();
    public async Task<Usuario?> Obtener(int IdUsuarioParam) => await _data.Obtener(IdUsuarioParam);
    public async Task<Usuario?> ObtenerPorLogin(string login) => await _data.ObtenerPorLogin(login);
    public async Task<List<Rol>> ObtenerRoles(int idUsuario) => await _data.ObtenerRoles(idUsuario);
    public async Task Crear(Usuario registro) => await _data.Crear(registro);
    public async Task Actualizar(Usuario registro) => await _data.Actualizar(registro);
    public async Task Eliminar(Usuario registro) => await _data.Eliminar(registro);
}

