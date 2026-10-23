using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class RRHH_FuncFamRespuestaLogica
{
    private readonly RRHH_FuncFamRespuestaData _data;

    public RRHH_FuncFamRespuestaLogica(RRHH_FuncFamRespuestaData data) => _data = data;

    public async Task<List<RRHH_FuncFamRespuesta>> Listar() => await _data.Listar();
    public async Task<RRHH_FuncFamRespuesta?> Obtener(int IdEmpleadoParam, byte IdPreguntaParam) => await _data.Obtener(IdEmpleadoParam, IdPreguntaParam);
    public async Task Crear(RRHH_FuncFamRespuesta registro) => await _data.Crear(registro);
    public async Task Actualizar(RRHH_FuncFamRespuesta registro) => await _data.Actualizar(registro);
    public async Task Eliminar(RRHH_FuncFamRespuesta registro) => await _data.Eliminar(registro);
}
