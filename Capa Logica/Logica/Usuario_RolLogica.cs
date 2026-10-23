using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class Usuario_RolLogica
{
    private readonly Usuario_RolData _data;

    public Usuario_RolLogica(Usuario_RolData data) => _data = data;

    public async Task<List<Usuario_Rol>> Listar() => await _data.Listar();
    public async Task<Usuario_Rol?> Obtener(int IdUsuarioParam, int IdRolParam) => await _data.Obtener(IdUsuarioParam, IdRolParam);
    public async Task Crear(Usuario_Rol registro) => await _data.Crear(registro);
    public async Task Actualizar(Usuario_Rol registro) => await _data.Actualizar(registro);
    public async Task Eliminar(Usuario_Rol registro) => await _data.Eliminar(registro);
}

