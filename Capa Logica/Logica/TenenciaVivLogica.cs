using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class TenenciaVivLogica
{
    private readonly TenenciaVivData _data;

    public TenenciaVivLogica(TenenciaVivData data) => _data = data;

    public async Task<List<TenenciaViv>> Listar() => await _data.Listar();
    public async Task<TenenciaViv?> Obtener(int IdTenenciaParam) => await _data.Obtener(IdTenenciaParam);
    public async Task Crear(TenenciaViv registro) => await _data.Crear(registro);
    public async Task Actualizar(TenenciaViv registro) => await _data.Actualizar(registro);
    public async Task Eliminar(TenenciaViv registro) => await _data.Eliminar(registro);
}

