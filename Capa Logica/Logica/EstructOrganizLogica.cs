using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class EstructOrganizLogica
{
    private readonly EstructOrganizData _data;

    public EstructOrganizLogica(EstructOrganizData data) => _data = data;

    public async Task<List<EstructOrganiz>> Listar() => await _data.Listar();
    public async Task<EstructOrganiz?> Obtener(int YearParam, int idAreaOrganizParam) => await _data.Obtener(YearParam, idAreaOrganizParam);
    public async Task Crear(EstructOrganiz registro) => await _data.Crear(registro);
    public async Task Actualizar(EstructOrganiz registro) => await _data.Actualizar(registro);
    public async Task Eliminar(EstructOrganiz registro) => await _data.Eliminar(registro);
}

