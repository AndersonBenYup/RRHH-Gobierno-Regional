using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class EstudiosRealizadoLogica
{
    private readonly EstudiosRealizadoData _data;

    public EstudiosRealizadoLogica(EstudiosRealizadoData data) => _data = data;

    public async Task<List<EstudiosRealizado>> Listar() => await _data.Listar();
    public async Task<EstudiosRealizado?> Obtener(int IdTipoEstudiosParam, int IdEmpleadoParam, int CorrelativoParam) => await _data.Obtener(IdTipoEstudiosParam, IdEmpleadoParam, CorrelativoParam);
    public async Task Crear(EstudiosRealizado registro) => await _data.Crear(registro);
    public async Task Actualizar(EstudiosRealizado registro) => await _data.Actualizar(registro);
    public async Task Eliminar(EstudiosRealizado registro) => await _data.Eliminar(registro);
}

