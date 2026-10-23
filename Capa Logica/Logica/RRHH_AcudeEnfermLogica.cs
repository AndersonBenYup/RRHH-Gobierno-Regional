using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class RRHH_AcudeEnfermLogica
{
    private readonly RRHH_AcudeEnfermData _data;

    public RRHH_AcudeEnfermLogica(RRHH_AcudeEnfermData data) => _data = data;

    public async Task<List<RRHH_AcudeEnferm>> Listar() => await _data.Listar();
    public async Task<RRHH_AcudeEnferm?> Obtener(int idAcudeEnfermParam) => await _data.Obtener(idAcudeEnfermParam);
    public async Task Crear(RRHH_AcudeEnferm registro) => await _data.Crear(registro);
    public async Task Actualizar(RRHH_AcudeEnferm registro) => await _data.Actualizar(registro);
    public async Task Eliminar(RRHH_AcudeEnferm registro) => await _data.Eliminar(registro);
}

