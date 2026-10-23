using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class ExpLaboralLogica
{
    private readonly ExpLaboralData _data;

    public ExpLaboralLogica(ExpLaboralData data) => _data = data;

    public async Task<List<ExpLaboral>> Listar() => await _data.Listar();
    public async Task<ExpLaboral?> Obtener(int IdEmpleadoParam, int IdExpLabParam) => await _data.Obtener(IdEmpleadoParam, IdExpLabParam);
    public async Task Crear(ExpLaboral registro) => await _data.Crear(registro);
    public async Task Actualizar(ExpLaboral registro) => await _data.Actualizar(registro);
    public async Task Eliminar(ExpLaboral registro) => await _data.Eliminar(registro);
}

