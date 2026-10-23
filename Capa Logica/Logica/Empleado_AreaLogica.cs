using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class Empleado_AreaLogica
{
    private readonly Empleado_AreaData _data;

    public Empleado_AreaLogica(Empleado_AreaData data) => _data = data;

    public async Task<List<Empleado_Area>> Listar() => await _data.Listar();
    public async Task<Empleado_Area?> Obtener(int YearParam, int idAreaOrganizParam, int IdEmpleadoParam) => await _data.Obtener(YearParam, idAreaOrganizParam, IdEmpleadoParam);
    public async Task Crear(Empleado_Area registro) => await _data.Crear(registro);
    public async Task Actualizar(Empleado_Area registro) => await _data.Actualizar(registro);
    public async Task Eliminar(Empleado_Area registro) => await _data.Eliminar(registro);
}

