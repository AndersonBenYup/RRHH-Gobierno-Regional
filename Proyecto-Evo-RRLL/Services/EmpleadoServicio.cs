using Capa_Entidades.Contratos;

namespace Proyecto_Evo_RRLL.Services;

public class EmpleadoServicio
{
    private readonly Capa_Logica.EmpleadoLogica _empleadoData;
    private readonly Capa_Logica.PersonaLogica _personaData;

    public EmpleadoServicio(Capa_Logica.EmpleadoLogica empleadoData, Capa_Logica.PersonaLogica personaData)
    {
        _empleadoData = empleadoData;
        _personaData = personaData;
    }

    public async Task<List<(int Id, string Nombre)>> EmpleadosAsync()
    {
        var empleados = await _empleadoData.Listar();
        var personas = await _personaData.Listar();
        var personasPorId = personas.ToDictionary(p => p.IdPersona);
        return empleados.Select(e =>
        {
            var persona = e.IdPersona is null ? null : personasPorId.GetValueOrDefault(e.IdPersona.Value);
            var nombre = persona is null
                ? $"(Empleado {e.IdEmpleado})"
                : $"{persona.Nombres} {persona.Apellido_Paterno} {persona.Apellido_Materno}".Trim();
            return (e.IdEmpleado, nombre);
        }).OrderBy(x => x.nombre, StringComparer.CurrentCultureIgnoreCase).ToList();
    }

    public async Task<string?> NombreEmpleadoAsync(int idEmpleado)
        => (await EmpleadosAsync()).FirstOrDefault(x => x.Id == idEmpleado).Nombre;
}