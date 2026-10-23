using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class RRHH_SaludLogica
{
    private readonly RRHH_SaludData _data;

    public RRHH_SaludLogica(RRHH_SaludData data) => _data = data;

    public async Task<List<RRHH_Salud>> Listar() => await _data.Listar();
    public async Task<RRHH_Salud?> Obtener(int idSaludParam) => await _data.Obtener(idSaludParam);
    public async Task Crear(RRHH_Salud registro) => await _data.Crear(registro);
    public async Task Actualizar(RRHH_Salud registro) => await _data.Actualizar(registro);
    public async Task Eliminar(RRHH_Salud registro) => await _data.Eliminar(registro);
}

