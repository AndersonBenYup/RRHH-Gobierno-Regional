using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class ResponsableXUOLogica
{
    private readonly ResponsableXUOData _data;

    public ResponsableXUOLogica(ResponsableXUOData data) => _data = data;

    public async Task<List<ResponsableXUO>> Listar() => await _data.Listar();
    public async Task<ResponsableXUO?> Obtener(int IdResponsableParam) => await _data.Obtener(IdResponsableParam);
    public async Task Crear(ResponsableXUO registro) => await _data.Crear(registro);
    public async Task Actualizar(ResponsableXUO registro) => await _data.Actualizar(registro);
    public async Task Eliminar(ResponsableXUO registro) => await _data.Eliminar(registro);
}

