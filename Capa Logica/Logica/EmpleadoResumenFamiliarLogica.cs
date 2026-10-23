using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class EmpleadoResumenFamiliarLogica
{
    private readonly EmpleadoResumenFamiliarData _data;

    public EmpleadoResumenFamiliarLogica(EmpleadoResumenFamiliarData data) => _data = data;

    public async Task<List<EmpleadoResumenFamiliar>> Listar() => await _data.Listar();
    public async Task<EmpleadoResumenFamiliar?> Obtener(int IdEmpleadoParam) => await _data.Obtener(IdEmpleadoParam);
}
