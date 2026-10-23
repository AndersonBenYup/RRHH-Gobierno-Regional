using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class EstadoCivilLogica
{
    private readonly EstadoCivilData _data;

    public EstadoCivilLogica(EstadoCivilData data) => _data = data;

    public async Task<List<EstadoCivil>> Listar() => await _data.Listar();
    public async Task<EstadoCivil?> Obtener(int IdEstadoCivilParam) => await _data.Obtener(IdEstadoCivilParam);
    public async Task Crear(EstadoCivil registro) => await _data.Crear(registro);
    public async Task Actualizar(EstadoCivil registro) => await _data.Actualizar(registro);
    public async Task Eliminar(EstadoCivil registro) => await _data.Eliminar(registro);
}

