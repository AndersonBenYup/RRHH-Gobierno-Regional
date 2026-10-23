using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class EstructOrganizDotacionLogica
{
    private readonly EstructOrganizDotacionData _data;

    public EstructOrganizDotacionLogica(EstructOrganizDotacionData data) => _data = data;

    public async Task<List<EstructOrganizDotacion>> Listar() => await _data.Listar();
    public async Task<EstructOrganizDotacion?> Obtener(int YearParam, int idAreaOrganizParam) => await _data.Obtener(YearParam, idAreaOrganizParam);
}
