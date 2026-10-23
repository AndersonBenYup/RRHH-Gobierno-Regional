using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class RRHH_TipoFamiliaLogica
{
    private readonly RRHH_TipoFamiliaData _data;

    public RRHH_TipoFamiliaLogica(RRHH_TipoFamiliaData data) => _data = data;

    public async Task<List<RRHH_TipoFamilia>> Listar() => await _data.Listar();
    public async Task<RRHH_TipoFamilia?> Obtener(int idTipoFamParam) => await _data.Obtener(idTipoFamParam);
    public async Task Crear(RRHH_TipoFamilia registro) => await _data.Crear(registro);
    public async Task Actualizar(RRHH_TipoFamilia registro) => await _data.Actualizar(registro);
    public async Task Eliminar(RRHH_TipoFamilia registro) => await _data.Eliminar(registro);
}

