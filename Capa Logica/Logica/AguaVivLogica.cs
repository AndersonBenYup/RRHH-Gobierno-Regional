using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class AguaVivLogica
{
    private readonly AguaVivData _data;

    public AguaVivLogica(AguaVivData data) => _data = data;

    public async Task<List<AguaViv>> Listar() => await _data.Listar();
    public async Task<AguaViv?> Obtener(int IdAguaParam) => await _data.Obtener(IdAguaParam);
    public async Task Crear(AguaViv registro) => await _data.Crear(registro);
    public async Task Actualizar(AguaViv registro) => await _data.Actualizar(registro);
    public async Task Eliminar(AguaViv registro) => await _data.Eliminar(registro);
}

