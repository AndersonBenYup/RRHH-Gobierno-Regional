using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class RegAsisDiarioLogica
{
    private readonly RegAsisDiarioData _data;

    public RegAsisDiarioLogica(RegAsisDiarioData data) => _data = data;

    public async Task<List<RegAsisDiario>> Listar() => await _data.Listar();
    public async Task<RegAsisDiario?> Obtener(DateTime FechaParam, int IdEmpleadoParam) => await _data.Obtener(FechaParam, IdEmpleadoParam);
    public async Task Crear(RegAsisDiario registro) => await _data.Crear(registro);
    public async Task Actualizar(RegAsisDiario registro) => await _data.Actualizar(registro);
    public async Task Eliminar(RegAsisDiario registro) => await _data.Eliminar(registro);
}

