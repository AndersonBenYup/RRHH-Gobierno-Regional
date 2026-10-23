using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class CargoLogica
{
    private readonly CargoData _data;

    public CargoLogica(CargoData data) => _data = data;

    public async Task<List<Cargo>> Listar() => await _data.Listar();
    public async Task<Cargo?> Obtener(int IdCargoParam) => await _data.Obtener(IdCargoParam);
    public async Task Crear(Cargo registro) => await _data.Crear(registro);
    public async Task Actualizar(Cargo registro) => await _data.Actualizar(registro);
    public async Task Eliminar(Cargo registro) => await _data.Eliminar(registro);
}

