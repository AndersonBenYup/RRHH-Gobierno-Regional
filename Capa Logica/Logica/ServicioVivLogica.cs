using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class ServicioVivLogica
{
    private readonly ServicioVivData _data;

    public ServicioVivLogica(ServicioVivData data) => _data = data;

    public async Task<List<ServicioViv>> Listar() => await _data.Listar();
    public async Task<ServicioViv?> Obtener(int IdServicioParam) => await _data.Obtener(IdServicioParam);
    public async Task Crear(ServicioViv registro) => await _data.Crear(registro);
    public async Task Actualizar(ServicioViv registro) => await _data.Actualizar(registro);
    public async Task Eliminar(ServicioViv registro) => await _data.Eliminar(registro);
}
