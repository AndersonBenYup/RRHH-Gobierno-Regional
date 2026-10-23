using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class ResolucionLogica
{
    private readonly ResolucionData _data;

    public ResolucionLogica(ResolucionData data) => _data = data;

    public async Task<List<Resolucion>> Listar() => await _data.Listar();
    public async Task<Resolucion?> Obtener(int idEmpleadoParam, int idResolucionParam) => await _data.Obtener(idEmpleadoParam, idResolucionParam);
    public async Task Crear(Resolucion registro) => await _data.Crear(registro);
    public async Task Actualizar(Resolucion registro) => await _data.Actualizar(registro);
    public async Task Eliminar(Resolucion registro) => await _data.Eliminar(registro);
}

