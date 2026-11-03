using Capa_Entidades;
using Capa_Entidades.Contratos;
using Capa_Entidades.Validacion;
using Capa_Logica.Reglas;

namespace Capa_Logica;

public class PersonaLogica : LogicaBase<Persona>
{
    private readonly IPersonaRepositorio _personas;

    public PersonaLogica(IPersonaRepositorio personas) : base(personas) => _personas = personas;

    public Task<Persona?> Obtener(int idPersona) => Repositorio.Obtener(idPersona);

    /// <summary>SC-008: nombres, formato del documento (DNI/RUC) y documento no duplicado.</summary>
    public async Task<List<ErrorValidacion>> Validar(Persona persona, bool exigirApellido = true)
    {
        var duplicado = !string.IsNullOrWhiteSpace(persona.NumDocID)
            && await _personas.ExisteDocumento(persona.NumDocID, persona.IdPersona == 0 ? null : persona.IdPersona);
        return ReglasPersona.Validar(persona, duplicado, exigirApellido);
    }
}
