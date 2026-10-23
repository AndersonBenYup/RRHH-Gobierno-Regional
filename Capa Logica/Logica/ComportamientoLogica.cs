using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class ComportamientoLogica
{
    private readonly ComportamientoData _data;

    public ComportamientoLogica(ComportamientoData data) => _data = data;

    public async Task<List<Comportamiento>> Listar() => await _data.Listar();
    public async Task<Comportamiento?> Obtener(int idComportamientoParam) => await _data.Obtener(idComportamientoParam);
    public async Task Crear(Comportamiento registro) => await _data.Crear(registro);
    public async Task Actualizar(Comportamiento registro) => await _data.Actualizar(registro);
    public async Task Eliminar(Comportamiento registro) => await _data.Eliminar(registro);
}

