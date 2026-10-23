using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class NacionalidadLogica
{
    private readonly NacionalidadData _data;

    public NacionalidadLogica(NacionalidadData data) => _data = data;

    public async Task<List<Nacionalidad>> Listar() => await _data.Listar();
    public async Task<Nacionalidad?> Obtener(int IdNacionalidadParam) => await _data.Obtener(IdNacionalidadParam);
    public async Task Crear(Nacionalidad registro) => await _data.Crear(registro);
    public async Task Actualizar(Nacionalidad registro) => await _data.Actualizar(registro);
    public async Task Eliminar(Nacionalidad registro) => await _data.Eliminar(registro);
}

