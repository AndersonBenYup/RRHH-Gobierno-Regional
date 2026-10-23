using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class RotacionLogica
{
    private readonly RotacionData _data;

    public RotacionLogica(RotacionData data) => _data = data;

    public async Task<List<Rotacion>> Listar() => await _data.Listar();
    public async Task<Rotacion?> Obtener(int idEmpleadoParam, int idRotacionParam) => await _data.Obtener(idEmpleadoParam, idRotacionParam);
    public async Task Crear(Rotacion registro) => await _data.Crear(registro);
    public async Task Actualizar(Rotacion registro) => await _data.Actualizar(registro);
    public async Task Eliminar(Rotacion registro) => await _data.Eliminar(registro);
}

