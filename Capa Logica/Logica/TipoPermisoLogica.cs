using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class TipoPermisoLogica
{
    private readonly TipoPermisoData _data;

    public TipoPermisoLogica(TipoPermisoData data) => _data = data;

    public async Task<List<TipoPermiso>> Listar() => await _data.Listar();
    public async Task<TipoPermiso?> Obtener(int idTipoPermisoParam) => await _data.Obtener(idTipoPermisoParam);
    public async Task Crear(TipoPermiso registro) => await _data.Crear(registro);
    public async Task Actualizar(TipoPermiso registro) => await _data.Actualizar(registro);
    public async Task Eliminar(TipoPermiso registro) => await _data.Eliminar(registro);
}

