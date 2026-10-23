using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class TipoResolucionLogica
{
    private readonly TipoResolucionData _data;

    public TipoResolucionLogica(TipoResolucionData data) => _data = data;

    public async Task<List<TipoResolucion>> Listar() => await _data.Listar();
    public async Task<TipoResolucion?> Obtener(int idTipoResolucionParam) => await _data.Obtener(idTipoResolucionParam);
    public async Task Crear(TipoResolucion registro) => await _data.Crear(registro);
    public async Task Actualizar(TipoResolucion registro) => await _data.Actualizar(registro);
    public async Task Eliminar(TipoResolucion registro) => await _data.Eliminar(registro);
}

