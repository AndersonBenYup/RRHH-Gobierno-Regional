using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class RRHH_RelacParejaLogica
{
    private readonly RRHH_RelacParejaData _data;

    public RRHH_RelacParejaLogica(RRHH_RelacParejaData data) => _data = data;

    public async Task<List<RRHH_RelacPareja>> Listar() => await _data.Listar();
    public async Task<RRHH_RelacPareja?> Obtener(int idRelacParejaParam) => await _data.Obtener(idRelacParejaParam);
    public async Task Crear(RRHH_RelacPareja registro) => await _data.Crear(registro);
    public async Task Actualizar(RRHH_RelacPareja registro) => await _data.Actualizar(registro);
    public async Task Eliminar(RRHH_RelacPareja registro) => await _data.Eliminar(registro);
}

