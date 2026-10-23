using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class TipoSangreLogica
{
    private readonly TipoSangreData _data;

    public TipoSangreLogica(TipoSangreData data) => _data = data;

    public async Task<List<TipoSangre>> Listar() => await _data.Listar();
    public async Task<TipoSangre?> Obtener(int IdTipoSangreParam) => await _data.Obtener(IdTipoSangreParam);
    public async Task Crear(TipoSangre registro) => await _data.Crear(registro);
    public async Task Actualizar(TipoSangre registro) => await _data.Actualizar(registro);
    public async Task Eliminar(TipoSangre registro) => await _data.Eliminar(registro);
}

