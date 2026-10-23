using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class RRHH_AseguradoLogica
{
    private readonly RRHH_AseguradoData _data;

    public RRHH_AseguradoLogica(RRHH_AseguradoData data) => _data = data;

    public async Task<List<RRHH_Asegurado>> Listar() => await _data.Listar();
    public async Task<RRHH_Asegurado?> Obtener(int idEmpleadoParam, int NroBeneficiarioParam) => await _data.Obtener(idEmpleadoParam, NroBeneficiarioParam);
    public async Task Crear(RRHH_Asegurado registro) => await _data.Crear(registro);
    public async Task Actualizar(RRHH_Asegurado registro) => await _data.Actualizar(registro);
    public async Task Eliminar(RRHH_Asegurado registro) => await _data.Eliminar(registro);
}

