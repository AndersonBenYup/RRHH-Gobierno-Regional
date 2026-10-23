using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class ExcretasVivLogica
{
    private readonly ExcretasVivData _data;

    public ExcretasVivLogica(ExcretasVivData data) => _data = data;

    public async Task<List<ExcretasViv>> Listar() => await _data.Listar();
    public async Task<ExcretasViv?> Obtener(int IdExcretasParam) => await _data.Obtener(IdExcretasParam);
    public async Task Crear(ExcretasViv registro) => await _data.Crear(registro);
    public async Task Actualizar(ExcretasViv registro) => await _data.Actualizar(registro);
    public async Task Eliminar(ExcretasViv registro) => await _data.Eliminar(registro);
}

