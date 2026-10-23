using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class PermisoLogica
{
    private readonly PermisoData _data;

    public PermisoLogica(PermisoData data) => _data = data;

    public async Task<List<Permiso>> Listar() => await _data.Listar();
    public async Task<Permiso?> Obtener(int IdEmpleadoParam, int NPermisoParam) => await _data.Obtener(IdEmpleadoParam, NPermisoParam);
    public async Task Crear(Permiso registro) => await _data.Crear(registro);
    public async Task Actualizar(Permiso registro) => await _data.Actualizar(registro);
    public async Task Eliminar(Permiso registro) => await _data.Eliminar(registro);
}

