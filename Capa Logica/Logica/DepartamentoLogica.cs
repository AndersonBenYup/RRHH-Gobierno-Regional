using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class DepartamentoLogica
{
    private readonly DepartamentoData _data;

    public DepartamentoLogica(DepartamentoData data) => _data = data;

    public async Task<List<Departamento>> Listar() => await _data.Listar();
    public async Task<Departamento?> Obtener(string CodDepartamentoParam) => await _data.Obtener(CodDepartamentoParam);
}
