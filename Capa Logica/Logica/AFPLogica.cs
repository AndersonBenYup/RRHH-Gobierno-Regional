using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class AFPLogica
{
    private readonly AFPData _data;

    public AFPLogica(AFPData data) => _data = data;

    public async Task<List<AFP>> Listar() => await _data.Listar();
    public async Task<AFP?> Obtener(int IdAFPParam) => await _data.Obtener(IdAFPParam);
    public async Task Crear(AFP registro) => await _data.Crear(registro);
    public async Task Actualizar(AFP registro) => await _data.Actualizar(registro);
    public async Task Eliminar(AFP registro) => await _data.Eliminar(registro);
}

