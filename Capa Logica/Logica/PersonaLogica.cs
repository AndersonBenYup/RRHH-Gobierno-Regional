using Capa_Entidades;
using Capa_Datos;
using Capa_Entidades.Validacion;
using Capa_Logica.Reglas;

namespace Capa_Logica;

public class PersonaLogica
{
    private readonly PersonaData _data;

    public PersonaLogica(PersonaData data) => _data = data;

    public async Task<List<Persona>> Listar() => await _data.Listar();
    public async Task<Persona?> Obtener(int IdPersonaParam) => await _data.Obtener(IdPersonaParam);
    /// <summary>SC-008: nombres, formato del documento (DNI/RUC) y documento no duplicado.</summary>
    public async Task<List<ErrorValidacion>> Validar(Persona persona, bool exigirApellido = true)
    {
        var duplicado = !string.IsNullOrWhiteSpace(persona.NumDocID)
            && await _data.ExisteDocumento(persona.NumDocID, persona.IdPersona == 0 ? null : persona.IdPersona);
        return ReglasPersona.Validar(persona, duplicado, exigirApellido);
    }

    public async Task Crear(Persona registro) => await _data.Crear(registro);
    public async Task Actualizar(Persona registro) => await _data.Actualizar(registro);
    public async Task Eliminar(Persona registro) => await _data.Eliminar(registro);
}

