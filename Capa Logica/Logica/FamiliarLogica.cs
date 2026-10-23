using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class FamiliarLogica
{
    private readonly FamiliarData _data;

    public FamiliarLogica(FamiliarData data) => _data = data;

    public async Task<List<Familiar>> Listar() => await _data.Listar();
    public async Task<Familiar?> Obtener(int IdEmpleadoParam, int IdPersonaParam) => await _data.Obtener(IdEmpleadoParam, IdPersonaParam);
    public async Task Crear(Familiar registro) => await _data.Crear(registro);
    public async Task Actualizar(Familiar registro) => await _data.Actualizar(registro);
    public async Task Eliminar(Familiar registro) => await _data.Eliminar(registro);
}

