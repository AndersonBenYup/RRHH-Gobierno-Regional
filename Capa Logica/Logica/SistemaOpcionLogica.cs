using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class SistemaOpcionLogica
{
    private readonly SistemaOpcionData _data;

    public SistemaOpcionLogica(SistemaOpcionData data) => _data = data;

    public async Task<List<SistemaOpcion>> Listar() => await _data.Listar();
    public async Task<SistemaOpcion?> Obtener(string IdSistemaOpcionParam) => await _data.Obtener(IdSistemaOpcionParam);
    public async Task Crear(SistemaOpcion registro) => await _data.Crear(registro);
    public async Task Actualizar(SistemaOpcion registro) => await _data.Actualizar(registro);
    public async Task Eliminar(SistemaOpcion registro) => await _data.Eliminar(registro);
}

