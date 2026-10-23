using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class DiscapacidadLogica
{
    private readonly DiscapacidadData _data;

    public DiscapacidadLogica(DiscapacidadData data) => _data = data;

    public async Task<List<Discapacidad>> Listar() => await _data.Listar();
    public async Task<Discapacidad?> Obtener(int IdDiscapacidadParam) => await _data.Obtener(IdDiscapacidadParam);
    public async Task Crear(Discapacidad registro) => await _data.Crear(registro);
    public async Task Actualizar(Discapacidad registro) => await _data.Actualizar(registro);
    public async Task Eliminar(Discapacidad registro) => await _data.Eliminar(registro);
}

