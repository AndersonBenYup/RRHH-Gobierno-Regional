using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class CapacitacionLogica
{
    private readonly CapacitacionData _data;

    public CapacitacionLogica(CapacitacionData data) => _data = data;

    public async Task<List<Capacitacion>> Listar() => await _data.Listar();
    public async Task<Capacitacion?> Obtener(int IdCapacitacionParam) => await _data.Obtener(IdCapacitacionParam);
    public async Task Crear(Capacitacion registro) => await _data.Crear(registro);
    public async Task Actualizar(Capacitacion registro) => await _data.Actualizar(registro);
    public async Task Eliminar(Capacitacion registro) => await _data.Eliminar(registro);
}

