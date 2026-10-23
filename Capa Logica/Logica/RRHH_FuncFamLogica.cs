using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class RRHH_FuncFamLogica
{
    private readonly RRHH_FuncFamData _data;

    public RRHH_FuncFamLogica(RRHH_FuncFamData data) => _data = data;

    public async Task<List<RRHH_FuncFam>> Listar() => await _data.Listar();
    public async Task<RRHH_FuncFam?> Obtener(int IdEmpleadoParam) => await _data.Obtener(IdEmpleadoParam);
    public async Task Crear(RRHH_FuncFam registro) => await _data.Crear(registro);
    public async Task Actualizar(RRHH_FuncFam registro) => await _data.Actualizar(registro);
    public async Task Eliminar(RRHH_FuncFam registro) => await _data.Eliminar(registro);
}

