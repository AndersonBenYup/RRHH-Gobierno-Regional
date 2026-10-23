using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class AlumbradoVivLogica
{
    private readonly AlumbradoVivData _data;

    public AlumbradoVivLogica(AlumbradoVivData data) => _data = data;

    public async Task<List<AlumbradoViv>> Listar() => await _data.Listar();
    public async Task<AlumbradoViv?> Obtener(int IdAlumbradoParam) => await _data.Obtener(IdAlumbradoParam);
    public async Task Crear(AlumbradoViv registro) => await _data.Crear(registro);
    public async Task Actualizar(AlumbradoViv registro) => await _data.Actualizar(registro);
    public async Task Eliminar(AlumbradoViv registro) => await _data.Eliminar(registro);
}

