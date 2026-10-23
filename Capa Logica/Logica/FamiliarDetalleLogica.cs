using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class FamiliarDetalleLogica
{
    private readonly FamiliarDetalleData _data;

    public FamiliarDetalleLogica(FamiliarDetalleData data) => _data = data;

    public async Task<List<FamiliarDetalle>> Listar() => await _data.Listar();
    public async Task<FamiliarDetalle?> Obtener(int IdEmpleadoParam, int IdPersonaParam) => await _data.Obtener(IdEmpleadoParam, IdPersonaParam);
}
