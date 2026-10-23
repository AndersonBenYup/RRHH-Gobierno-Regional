using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class UbigeoLogica
{
    private readonly UbigeoData _data;

    public UbigeoLogica(UbigeoData data) => _data = data;

    public async Task<List<Ubigeo>> Listar() => await _data.Listar();
    public async Task<Ubigeo?> Obtener(string IdUbigeoParam) => await _data.Obtener(IdUbigeoParam);
    public async Task Crear(Ubigeo registro) => await _data.Crear(registro);
    public async Task Actualizar(Ubigeo registro) => await _data.Actualizar(registro);
    public async Task Eliminar(Ubigeo registro) => await _data.Eliminar(registro);
}

