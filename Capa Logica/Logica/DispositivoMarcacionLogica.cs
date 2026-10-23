using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class DispositivoMarcacionLogica
{
    private readonly DispositivoMarcacionData _data;

    public DispositivoMarcacionLogica(DispositivoMarcacionData data) => _data = data;

    public async Task<List<DispositivoMarcacion>> Listar() => await _data.Listar();
    public async Task<DispositivoMarcacion?> Obtener(string CodigoParam) => await _data.Obtener(CodigoParam);
    public async Task Crear(DispositivoMarcacion registro) => await _data.Crear(registro);
    public async Task Actualizar(DispositivoMarcacion registro) => await _data.Actualizar(registro);
    public async Task Eliminar(DispositivoMarcacion registro) => await _data.Eliminar(registro);
}
