using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class TitulosEmpleadoLogica
{
    private readonly TitulosEmpleadoData _data;

    public TitulosEmpleadoLogica(TitulosEmpleadoData data) => _data = data;

    public async Task<List<TitulosEmpleado>> Listar() => await _data.Listar();
    public async Task<TitulosEmpleado?> Obtener(int IdTituloParam) => await _data.Obtener(IdTituloParam);
    public async Task Crear(TitulosEmpleado registro) => await _data.Crear(registro);
    public async Task Actualizar(TitulosEmpleado registro) => await _data.Actualizar(registro);
    public async Task Eliminar(TitulosEmpleado registro) => await _data.Eliminar(registro);
}

