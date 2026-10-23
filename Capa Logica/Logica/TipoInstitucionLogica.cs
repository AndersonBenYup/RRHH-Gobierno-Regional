using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class TipoInstitucionLogica
{
    private readonly TipoInstitucionData _data;

    public TipoInstitucionLogica(TipoInstitucionData data) => _data = data;

    public async Task<List<TipoInstitucion>> Listar() => await _data.Listar();
    public async Task<TipoInstitucion?> Obtener(int IdTipoInstitucionParam) => await _data.Obtener(IdTipoInstitucionParam);
    public async Task Crear(TipoInstitucion registro) => await _data.Crear(registro);
    public async Task Actualizar(TipoInstitucion registro) => await _data.Actualizar(registro);
    public async Task Eliminar(TipoInstitucion registro) => await _data.Eliminar(registro);
}

