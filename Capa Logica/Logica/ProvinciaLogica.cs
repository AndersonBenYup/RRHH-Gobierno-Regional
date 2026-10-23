using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class ProvinciaLogica
{
    private readonly ProvinciaData _data;

    public ProvinciaLogica(ProvinciaData data) => _data = data;

    public async Task<List<Provincia>> Listar() => await _data.Listar();
    public async Task<Provincia?> Obtener(string CodDepartamentoParam, string CodProvinciaParam) => await _data.Obtener(CodDepartamentoParam, CodProvinciaParam);
}
