using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class Rol_AccesoLogica
{
    private readonly Rol_AccesoData _data;

    public Rol_AccesoLogica(Rol_AccesoData data) => _data = data;

    public async Task<List<Rol_Acceso>> Listar() => await _data.Listar();
    public async Task<Rol_Acceso?> Obtener(int IdRolParam, string IdSistemaOpcionParam) => await _data.Obtener(IdRolParam, IdSistemaOpcionParam);
    public async Task Crear(Rol_Acceso registro) => await _data.Crear(registro);
    public async Task Actualizar(Rol_Acceso registro) => await _data.Actualizar(registro);
    public async Task Eliminar(Rol_Acceso registro) => await _data.Eliminar(registro);
}

