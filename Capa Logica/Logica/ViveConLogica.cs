using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class ViveConLogica
{
    private readonly ViveConData _data;

    public ViveConLogica(ViveConData data) => _data = data;

    public async Task<List<ViveCon>> Listar() => await _data.Listar();
    public async Task<ViveCon?> Obtener(int IdViveConParam) => await _data.Obtener(IdViveConParam);
    public async Task Crear(ViveCon registro) => await _data.Crear(registro);
    public async Task Actualizar(ViveCon registro) => await _data.Actualizar(registro);
    public async Task Eliminar(ViveCon registro) => await _data.Eliminar(registro);
}

