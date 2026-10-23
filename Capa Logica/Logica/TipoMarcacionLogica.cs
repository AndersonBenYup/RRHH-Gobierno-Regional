using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class TipoMarcacionLogica
{
    private readonly TipoMarcacionData _data;

    public TipoMarcacionLogica(TipoMarcacionData data) => _data = data;

    public async Task<List<TipoMarcacion>> Listar() => await _data.Listar();
    public async Task<TipoMarcacion?> Obtener(string CodigoParam) => await _data.Obtener(CodigoParam);
    public async Task Crear(TipoMarcacion registro) => await _data.Crear(registro);
    public async Task Actualizar(TipoMarcacion registro) => await _data.Actualizar(registro);
    public async Task Eliminar(TipoMarcacion registro) => await _data.Eliminar(registro);
}
