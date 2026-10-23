using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class EmpleadoAreaActualLogica
{
    private readonly EmpleadoAreaActualData _data;

    public EmpleadoAreaActualLogica(EmpleadoAreaActualData data) => _data = data;

    public async Task<List<EmpleadoAreaActual>> Listar() => await _data.Listar();
    public async Task<EmpleadoAreaActual?> Obtener(int IdEmpleadoParam) => await _data.Obtener(IdEmpleadoParam);
}
