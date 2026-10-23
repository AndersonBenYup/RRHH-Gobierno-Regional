using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class TipoFamiliarLogica
{
    private readonly TipoFamiliarData _data;

    public TipoFamiliarLogica(TipoFamiliarData data) => _data = data;

    public async Task<List<TipoFamiliar>> Listar() => await _data.Listar();
    public async Task<TipoFamiliar?> Obtener(int IdTipoFamParam) => await _data.Obtener(IdTipoFamParam);
    public async Task Crear(TipoFamiliar registro) => await _data.Crear(registro);
    public async Task Actualizar(TipoFamiliar registro) => await _data.Actualizar(registro);
    public async Task Eliminar(TipoFamiliar registro) => await _data.Eliminar(registro);
}

