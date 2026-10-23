using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class LocalLogica
{
    private readonly LocalData _data;

    public LocalLogica(LocalData data) => _data = data;

    public async Task<List<Local>> Listar() => await _data.Listar();
    public async Task<Local?> Obtener(int idLocalParam) => await _data.Obtener(idLocalParam);
    public async Task Crear(Local registro) => await _data.Crear(registro);
    public async Task Actualizar(Local registro) => await _data.Actualizar(registro);
    public async Task Eliminar(Local registro) => await _data.Eliminar(registro);
}

