using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class RRHH_RelacPHLogica
{
    private readonly RRHH_RelacPHData _data;

    public RRHH_RelacPHLogica(RRHH_RelacPHData data) => _data = data;

    public async Task<List<RRHH_RelacPH>> Listar() => await _data.Listar();
    public async Task<RRHH_RelacPH?> Obtener(int idRelacPHParam) => await _data.Obtener(idRelacPHParam);
    public async Task Crear(RRHH_RelacPH registro) => await _data.Crear(registro);
    public async Task Actualizar(RRHH_RelacPH registro) => await _data.Actualizar(registro);
    public async Task Eliminar(RRHH_RelacPH registro) => await _data.Eliminar(registro);
}

