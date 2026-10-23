using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class ViviendaServicioLogica
{
    private readonly ViviendaServicioData _data;

    public ViviendaServicioLogica(ViviendaServicioData data) => _data = data;

    public async Task<List<ViviendaServicio>> Listar() => await _data.Listar();
    public async Task<ViviendaServicio?> Obtener(int IdViviendaParam, int IdServicioParam) => await _data.Obtener(IdViviendaParam, IdServicioParam);
    public async Task Crear(ViviendaServicio registro) => await _data.Crear(registro);
    public async Task Actualizar(ViviendaServicio registro) => await _data.Actualizar(registro);
    public async Task Eliminar(ViviendaServicio registro) => await _data.Eliminar(registro);
}
