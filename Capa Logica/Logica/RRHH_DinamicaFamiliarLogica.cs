using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class RRHH_DinamicaFamiliarLogica
{
    private readonly RRHH_DinamicaFamiliarData _data;

    public RRHH_DinamicaFamiliarLogica(RRHH_DinamicaFamiliarData data) => _data = data;

    public async Task<List<RRHH_DinamicaFamiliar>> Listar() => await _data.Listar();
    public async Task<RRHH_DinamicaFamiliar?> Obtener(int idDinamicaParam) => await _data.Obtener(idDinamicaParam);
    public async Task Crear(RRHH_DinamicaFamiliar registro) => await _data.Crear(registro);
    public async Task Actualizar(RRHH_DinamicaFamiliar registro) => await _data.Actualizar(registro);
    public async Task Eliminar(RRHH_DinamicaFamiliar registro) => await _data.Eliminar(registro);
}

