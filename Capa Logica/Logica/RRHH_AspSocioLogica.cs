using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class RRHH_AspSocioLogica
{
    private readonly RRHH_AspSocioData _data;

    public RRHH_AspSocioLogica(RRHH_AspSocioData data) => _data = data;

    public async Task<List<RRHH_AspSocio>> Listar() => await _data.Listar();
    public async Task<RRHH_AspSocio?> Obtener(int idAspSocioParam) => await _data.Obtener(idAspSocioParam);
    public async Task Crear(RRHH_AspSocio registro) => await _data.Crear(registro);
    public async Task Actualizar(RRHH_AspSocio registro) => await _data.Actualizar(registro);
    public async Task Eliminar(RRHH_AspSocio registro) => await _data.Eliminar(registro);
}

