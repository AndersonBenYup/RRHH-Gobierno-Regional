using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class HorarioTemporalLogica
{
    private readonly HorarioTemporalData _data;

    public HorarioTemporalLogica(HorarioTemporalData data) => _data = data;

    public async Task<List<HorarioTemporal>> Listar() => await _data.Listar();
    public async Task<HorarioTemporal?> Obtener(int idEmpleadoParam, int NParam) => await _data.Obtener(idEmpleadoParam, NParam);
    public async Task Crear(HorarioTemporal registro) => await _data.Crear(registro);
    public async Task Actualizar(HorarioTemporal registro) => await _data.Actualizar(registro);
    public async Task Eliminar(HorarioTemporal registro) => await _data.Eliminar(registro);
}

