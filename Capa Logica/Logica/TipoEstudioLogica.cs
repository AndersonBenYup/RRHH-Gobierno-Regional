using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class TipoEstudioLogica
{
    private readonly TipoEstudioData _data;

    public TipoEstudioLogica(TipoEstudioData data) => _data = data;

    public async Task<List<TipoEstudio>> Listar() => await _data.Listar();
    public async Task<TipoEstudio?> Obtener(int IdTipoEstudiosParam) => await _data.Obtener(IdTipoEstudiosParam);
    public async Task Crear(TipoEstudio registro) => await _data.Crear(registro);
    public async Task Actualizar(TipoEstudio registro) => await _data.Actualizar(registro);
    public async Task Eliminar(TipoEstudio registro) => await _data.Eliminar(registro);
}

