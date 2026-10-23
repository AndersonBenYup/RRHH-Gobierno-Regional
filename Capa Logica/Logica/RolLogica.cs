using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class RolLogica
{
    private readonly RolData _data;

    public RolLogica(RolData data) => _data = data;

    public async Task<List<Rol>> Listar() => await _data.Listar();
    public async Task<Rol?> Obtener(int IdRolParam) => await _data.Obtener(IdRolParam);
    public async Task Crear(Rol registro) => await _data.Crear(registro);
    public async Task Actualizar(Rol registro) => await _data.Actualizar(registro);
    public async Task Eliminar(Rol registro) => await _data.Eliminar(registro);
}

