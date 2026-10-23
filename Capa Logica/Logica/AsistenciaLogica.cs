using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class AsistenciaLogica
{
    private readonly AsistenciaData _data;

    public AsistenciaLogica(AsistenciaData data) => _data = data;

    public async Task<List<Asistencia>> Listar() => await _data.Listar();
    public async Task<int> ContarEntreFechas(DateTime desde, DateTime hasta) => await _data.ContarEntreFechas(desde, hasta);
    public async Task<Asistencia?> Obtener(DateTime FechaParam, int IdEmpleadoParam) => await _data.Obtener(FechaParam, IdEmpleadoParam);
    public async Task Crear(Asistencia registro) => await _data.Crear(registro);
    public async Task Actualizar(Asistencia registro) => await _data.Actualizar(registro);
    public async Task Eliminar(Asistencia registro) => await _data.Eliminar(registro);
}

