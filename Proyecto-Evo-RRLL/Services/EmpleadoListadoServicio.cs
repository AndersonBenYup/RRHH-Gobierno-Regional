using Capa_Logica;
using Proyecto_Evo_RRLL.Models.ViewModels;

namespace Proyecto_Evo_RRLL.Services;

/// <summary>
/// Listado de Personal (pantalla y exportación). Antes vivía dentro de EmpleadoController (SRP):
/// reúne empleado, persona, cargo y área actual y aplica la búsqueda por nombre o documento.
/// </summary>
public class EmpleadoListadoServicio
{
    private readonly EmpleadoLogica _empleados;
    private readonly PersonaLogica _personas;
    private readonly CargoLogica _cargos;
    private readonly EmpleadoAreaActualLogica _areasActuales;

    public EmpleadoListadoServicio(EmpleadoLogica empleados, PersonaLogica personas, CargoLogica cargos,
        EmpleadoAreaActualLogica areasActuales)
    {
        _empleados = empleados;
        _personas = personas;
        _cargos = cargos;
        _areasActuales = areasActuales;
    }

    /// <summary>Filas ordenadas por Id; con <paramref name="busqueda"/>, solo las que coinciden en nombre o documento.</summary>
    public async Task<List<EmpleadoFila>> ListarAsync(string? busqueda)
    {
        var personasPorId = (await _personas.Listar()).ToDictionary(p => p.IdPersona);
        var cargosPorId = (await _cargos.Listar()).ToDictionary(c => c.IdCargo);
        var areasPorEmpleado = (await _areasActuales.Listar()).ToDictionary(a => a.IdEmpleado);

        var filas = new List<EmpleadoFila>();
        foreach (var e in (await _empleados.Listar()).OrderBy(e => e.IdEmpleado))
        {
            var persona = e.IdPersona is null ? null : personasPorId.GetValueOrDefault(e.IdPersona.Value);
            var nombre = persona is null ? null : $"{persona.Nombres} {persona.Apellido_Paterno} {persona.Apellido_Materno}".Trim();

            if (!string.IsNullOrWhiteSpace(busqueda)
                && nombre?.Contains(busqueda, StringComparison.OrdinalIgnoreCase) != true
                && persona?.NumDocID?.Contains(busqueda, StringComparison.OrdinalIgnoreCase) != true)
                continue;

            filas.Add(new EmpleadoFila
            {
                IdEmpleado = e.IdEmpleado,
                Nombres = nombre,
                NumDoc = persona?.NumDocID,
                Cargo = e.IdCargo is null ? null : cargosPorId.GetValueOrDefault(e.IdCargo.Value)?.Descripcion,
                Area = areasPorEmpleado.GetValueOrDefault(e.IdEmpleado)?.AreaOrganizacional,
                Estado = EmpleadoFichaServicio.EstadoTexto(e.Estado),
                FechaIngreso = e.FechaIngreso,
                TieneFoto = persona?.Foto is not null
            });
        }
        return filas;
    }

    /// <summary>Filas para exportar (encabezado incluido), con el mismo filtro que la pantalla.</summary>
    public async Task<List<string?[]>> FilasExportacionAsync(string? busqueda)
    {
        var filas = new List<string?[]> { new[] { "IdEmpleado", "Nombres", "NumDoc", "Cargo", "Area", "Estado", "FechaIngreso" } };
        filas.AddRange((await ListarAsync(busqueda)).Select(f => new[]
        {
            f.IdEmpleado.ToString(),
            f.Nombres ?? string.Empty,
            f.NumDoc ?? string.Empty,
            f.Cargo ?? string.Empty,
            f.Area ?? string.Empty,
            f.Estado ?? string.Empty,
            f.FechaIngreso?.ToString("dd/MM/yyyy") ?? string.Empty
        }));
        return filas;
    }
}
