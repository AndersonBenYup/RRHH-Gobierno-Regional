using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class TipoComportamientoLogica
{
    private readonly TipoComportamientoData _data;

    public TipoComportamientoLogica(TipoComportamientoData data) => _data = data;

    public async Task<List<TipoComportamiento>> Listar() => await _data.Listar();
    public async Task<TipoComportamiento?> Obtener(int idTipoComportamientoParam) => await _data.Obtener(idTipoComportamientoParam);
    public async Task Crear(TipoComportamiento registro) => await _data.Crear(registro);
    public async Task Actualizar(TipoComportamiento registro) => await _data.Actualizar(registro);
    public async Task Eliminar(TipoComportamiento registro) => await _data.Eliminar(registro);
}

