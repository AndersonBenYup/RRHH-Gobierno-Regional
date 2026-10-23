using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class MaterialVivLogica
{
    private readonly MaterialVivData _data;

    public MaterialVivLogica(MaterialVivData data) => _data = data;

    public async Task<List<MaterialViv>> Listar() => await _data.Listar();
    public async Task<MaterialViv?> Obtener(int IdMaterialParam) => await _data.Obtener(IdMaterialParam);
    public async Task Crear(MaterialViv registro) => await _data.Crear(registro);
    public async Task Actualizar(MaterialViv registro) => await _data.Actualizar(registro);
    public async Task Eliminar(MaterialViv registro) => await _data.Eliminar(registro);
}

