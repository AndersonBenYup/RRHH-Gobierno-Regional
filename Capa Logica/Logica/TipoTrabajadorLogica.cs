using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class TipoTrabajadorLogica
{
    private readonly TipoTrabajadorData _data;

    public TipoTrabajadorLogica(TipoTrabajadorData data) => _data = data;

    public async Task<List<TipoTrabajador>> Listar() => await _data.Listar();
    public async Task<TipoTrabajador?> Obtener(int IdTipoTrabajadorParam) => await _data.Obtener(IdTipoTrabajadorParam);
    public async Task Crear(TipoTrabajador registro) => await _data.Crear(registro);
    public async Task Actualizar(TipoTrabajador registro) => await _data.Actualizar(registro);
    public async Task Eliminar(TipoTrabajador registro) => await _data.Eliminar(registro);
}

