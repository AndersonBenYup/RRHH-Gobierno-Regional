using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class EstadoAsistenciaLogica
{
    private readonly EstadoAsistenciaData _data;

    public EstadoAsistenciaLogica(EstadoAsistenciaData data) => _data = data;

    public async Task<List<EstadoAsistencia>> Listar() => await _data.Listar();
    public async Task<EstadoAsistencia?> Obtener(string CodigoParam) => await _data.Obtener(CodigoParam);
    public async Task Crear(EstadoAsistencia registro) => await _data.Crear(registro);
    public async Task Actualizar(EstadoAsistencia registro) => await _data.Actualizar(registro);
    public async Task Eliminar(EstadoAsistencia registro) => await _data.Eliminar(registro);
}
