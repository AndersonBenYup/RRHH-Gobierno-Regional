using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class MotivoBajaLogica
{
    private readonly MotivoBajaData _data;

    public MotivoBajaLogica(MotivoBajaData data) => _data = data;

    public async Task<List<MotivoBaja>> Listar() => await _data.Listar();
    public async Task<MotivoBaja?> Obtener(int idMotivoBajaParam) => await _data.Obtener(idMotivoBajaParam);
    public async Task Crear(MotivoBaja registro) => await _data.Crear(registro);
    public async Task Actualizar(MotivoBaja registro) => await _data.Actualizar(registro);
    public async Task Eliminar(MotivoBaja registro) => await _data.Eliminar(registro);
}

