using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class RRHH_RelacHermanoLogica
{
    private readonly RRHH_RelacHermanoData _data;

    public RRHH_RelacHermanoLogica(RRHH_RelacHermanoData data) => _data = data;

    public async Task<List<RRHH_RelacHermano>> Listar() => await _data.Listar();
    public async Task<RRHH_RelacHermano?> Obtener(int idRelacHermanoParam) => await _data.Obtener(idRelacHermanoParam);
    public async Task Crear(RRHH_RelacHermano registro) => await _data.Crear(registro);
    public async Task Actualizar(RRHH_RelacHermano registro) => await _data.Actualizar(registro);
    public async Task Eliminar(RRHH_RelacHermano registro) => await _data.Eliminar(registro);
}

