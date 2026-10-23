using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class PersonaLogica
{
    private readonly PersonaData _data;

    public PersonaLogica(PersonaData data) => _data = data;

    public async Task<List<Persona>> Listar() => await _data.Listar();
    public async Task<Persona?> Obtener(int IdPersonaParam) => await _data.Obtener(IdPersonaParam);
    public async Task Crear(Persona registro) => await _data.Crear(registro);
    public async Task Actualizar(Persona registro) => await _data.Actualizar(registro);
    public async Task Eliminar(Persona registro) => await _data.Eliminar(registro);
}

