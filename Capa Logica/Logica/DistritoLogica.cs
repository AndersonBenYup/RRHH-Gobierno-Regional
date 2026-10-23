using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class DistritoLogica
{
    private readonly DistritoData _data;

    public DistritoLogica(DistritoData data) => _data = data;

    public async Task<List<Distrito>> Listar() => await _data.Listar();
    public async Task<Distrito?> Obtener(string IdUbigeoParam) => await _data.Obtener(IdUbigeoParam);
}
