using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class VinculoAseguradoLogica
{
    private readonly VinculoAseguradoData _data;

    public VinculoAseguradoLogica(VinculoAseguradoData data) => _data = data;

    public async Task<List<VinculoAsegurado>> Listar() => await _data.Listar();
    public async Task<VinculoAsegurado?> Obtener(int IdVinculoAseguradoParam) => await _data.Obtener(IdVinculoAseguradoParam);
    public async Task Crear(VinculoAsegurado registro) => await _data.Crear(registro);
    public async Task Actualizar(VinculoAsegurado registro) => await _data.Actualizar(registro);
    public async Task Eliminar(VinculoAsegurado registro) => await _data.Eliminar(registro);
}
