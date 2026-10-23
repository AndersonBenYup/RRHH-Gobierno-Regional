using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class PeriodoLaboralLogica
{
    private readonly PeriodoLaboralData _data;

    public PeriodoLaboralLogica(PeriodoLaboralData data) => _data = data;

    public async Task<List<PeriodoLaboral>> Listar() => await _data.Listar();
    public async Task<PeriodoLaboral?> Obtener(int IdEmpleadoParam, int NroPeriodoParam) => await _data.Obtener(IdEmpleadoParam, NroPeriodoParam);
    public async Task Crear(PeriodoLaboral registro) => await _data.Crear(registro);
    public async Task Actualizar(PeriodoLaboral registro) => await _data.Actualizar(registro);
    public async Task Eliminar(PeriodoLaboral registro) => await _data.Eliminar(registro);
}

