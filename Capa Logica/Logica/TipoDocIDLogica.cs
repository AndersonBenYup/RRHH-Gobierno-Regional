using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class TipoDocIDLogica
{
    private readonly TipoDocIDData _data;

    public TipoDocIDLogica(TipoDocIDData data) => _data = data;

    public async Task<List<TipoDocID>> Listar() => await _data.Listar();
    public async Task<TipoDocID?> Obtener(int TipoDocIDParam) => await _data.Obtener(TipoDocIDParam);
    public async Task Crear(TipoDocID registro) => await _data.Crear(registro);
    public async Task Actualizar(TipoDocID registro) => await _data.Actualizar(registro);
    public async Task Eliminar(TipoDocID registro) => await _data.Eliminar(registro);
}

