using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class RRHH_FeriadoLogica
{
    private readonly RRHH_FeriadoData _data;

    public RRHH_FeriadoLogica(RRHH_FeriadoData data) => _data = data;

    public async Task<List<RRHH_Feriado>> Listar() => await _data.Listar();
    public async Task<RRHH_Feriado?> Obtener(int idFeriadoParam) => await _data.Obtener(idFeriadoParam);
    public async Task Crear(RRHH_Feriado registro) => await _data.Crear(registro);
    public async Task Actualizar(RRHH_Feriado registro) => await _data.Actualizar(registro);
    public async Task Eliminar(RRHH_Feriado registro) => await _data.Eliminar(registro);
}

