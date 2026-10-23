using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class ViviendaLogica
{
    private readonly ViviendaData _data;

    public ViviendaLogica(ViviendaData data) => _data = data;

    public async Task<List<Vivienda>> Listar() => await _data.Listar();
    public async Task<Vivienda?> Obtener(int IdViviendaParam) => await _data.Obtener(IdViviendaParam);
    public async Task Crear(Vivienda registro) => await _data.Crear(registro);
    public async Task Actualizar(Vivienda registro) => await _data.Actualizar(registro);
    public async Task Eliminar(Vivienda registro) => await _data.Eliminar(registro);
}

