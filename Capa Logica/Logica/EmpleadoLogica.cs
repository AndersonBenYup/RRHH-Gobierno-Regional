using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class EmpleadoLogica
{
    private readonly EmpleadoData _data;

    public EmpleadoLogica(EmpleadoData data) => _data = data;

    public async Task<List<Empleado>> Listar() => await _data.Listar();
    public async Task<Empleado?> Obtener(int IdEmpleadoParam) => await _data.Obtener(IdEmpleadoParam);
    public async Task Crear(Empleado registro) => await _data.Crear(registro);
    public async Task Actualizar(Empleado registro) => await _data.Actualizar(registro);
    public async Task Eliminar(Empleado registro) => await _data.Eliminar(registro);
}

