using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class TipoVivLogica
{
    private readonly TipoVivData _data;

    public TipoVivLogica(TipoVivData data) => _data = data;

    public async Task<List<TipoViv>> Listar() => await _data.Listar();
    public async Task<TipoViv?> Obtener(int IdTipoParam) => await _data.Obtener(IdTipoParam);
    public async Task Crear(TipoViv registro) => await _data.Crear(registro);
    public async Task Actualizar(TipoViv registro) => await _data.Actualizar(registro);
    public async Task Eliminar(TipoViv registro) => await _data.Eliminar(registro);
}

