using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class EstadoEstudioLogica
{
    private readonly EstadoEstudioData _data;

    public EstadoEstudioLogica(EstadoEstudioData data) => _data = data;

    public async Task<List<EstadoEstudio>> Listar() => await _data.Listar();
    public async Task<EstadoEstudio?> Obtener(int IdEstadoEstudioParam) => await _data.Obtener(IdEstadoEstudioParam);
    public async Task Crear(EstadoEstudio registro) => await _data.Crear(registro);
    public async Task Actualizar(EstadoEstudio registro) => await _data.Actualizar(registro);
    public async Task Eliminar(EstadoEstudio registro) => await _data.Eliminar(registro);
}

