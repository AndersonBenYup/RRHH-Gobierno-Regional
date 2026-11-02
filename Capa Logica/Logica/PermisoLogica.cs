using Capa_Entidades;
using Capa_Datos;
using Capa_Entidades.Validacion;
using Capa_Logica.Reglas;

namespace Capa_Logica;

public class PermisoLogica
{
    private readonly PermisoData _data;

    public PermisoLogica(PermisoData data) => _data = data;

    public async Task<List<Permiso>> Listar() => await _data.Listar();
    public async Task<Permiso?> Obtener(int IdEmpleadoParam, int NPermisoParam) => await _data.Obtener(IdEmpleadoParam, NPermisoParam);
    /// <summary>SC-008: reglas de negocio (fechas, horas y solapamiento) que se revisan antes de guardar.</summary>
    public async Task<List<ErrorValidacion>> Validar(Permiso permiso)
        => ReglasPermiso.Validar(permiso, await _data.ListarPorEmpleado(permiso.IdEmpleado));

    public async Task Crear(Permiso registro) => await _data.Crear(registro);
    public async Task Actualizar(Permiso registro) => await _data.Actualizar(registro);
    public async Task Eliminar(Permiso registro) => await _data.Eliminar(registro);
}

