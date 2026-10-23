using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class RegimenPensionLogica
{
    private readonly RegimenPensionData _data;

    public RegimenPensionLogica(RegimenPensionData data) => _data = data;

    public async Task<List<RegimenPension>> Listar() => await _data.Listar();
    public async Task<RegimenPension?> Obtener(int IdRegimenPenParam) => await _data.Obtener(IdRegimenPenParam);
    public async Task Crear(RegimenPension registro) => await _data.Crear(registro);
    public async Task Actualizar(RegimenPension registro) => await _data.Actualizar(registro);
    public async Task Eliminar(RegimenPension registro) => await _data.Eliminar(registro);
}

