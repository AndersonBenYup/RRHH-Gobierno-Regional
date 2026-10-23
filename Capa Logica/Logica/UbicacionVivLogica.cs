using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class UbicacionVivLogica
{
    private readonly UbicacionVivData _data;

    public UbicacionVivLogica(UbicacionVivData data) => _data = data;

    public async Task<List<UbicacionViv>> Listar() => await _data.Listar();
    public async Task<UbicacionViv?> Obtener(int IdUbicacionParam) => await _data.Obtener(IdUbicacionParam);
    public async Task Crear(UbicacionViv registro) => await _data.Crear(registro);
    public async Task Actualizar(UbicacionViv registro) => await _data.Actualizar(registro);
    public async Task Eliminar(UbicacionViv registro) => await _data.Eliminar(registro);
}

