using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class MarcacionLogica
{
    private readonly MarcacionData _data;

    public MarcacionLogica(MarcacionData data) => _data = data;

    public async Task<List<Marcacion>> Listar() => await _data.Listar();
    public async Task<Marcacion?> Obtener(DateTime FechaParam, int IdEmpleadoParam) => await _data.Obtener(FechaParam, IdEmpleadoParam);
    public async Task Crear(Marcacion registro) => await _data.Crear(registro);
    public async Task Actualizar(Marcacion registro) => await _data.Actualizar(registro);
    public async Task Eliminar(Marcacion registro) => await _data.Eliminar(registro);
}

