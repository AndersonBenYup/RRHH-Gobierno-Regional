using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class ConservacionVivLogica
{
    private readonly ConservacionVivData _data;

    public ConservacionVivLogica(ConservacionVivData data) => _data = data;

    public async Task<List<ConservacionViv>> Listar() => await _data.Listar();
    public async Task<ConservacionViv?> Obtener(int IdConservacionParam) => await _data.Obtener(IdConservacionParam);
    public async Task Crear(ConservacionViv registro) => await _data.Crear(registro);
    public async Task Actualizar(ConservacionViv registro) => await _data.Actualizar(registro);
    public async Task Eliminar(ConservacionViv registro) => await _data.Eliminar(registro);
}

