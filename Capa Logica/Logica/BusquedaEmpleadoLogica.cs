using Capa_Entidades;
using Capa_Entidades.Contratos;
using Capa_Logica.Reglas;

namespace Capa_Logica;

/// <summary>
/// SC-031: busca empleados por número de documento para los desplegables con buscador.
/// Filtra en la BD (SC-005): primero las personas cuyo documento contiene los dígitos, luego sus vínculos de empleado.
/// Devuelve solo los Id de empleado; el documento no sale del servidor.
/// </summary>
public class BusquedaEmpleadoLogica
{
    private readonly IRepositorio<Persona> _personas;
    private readonly IRepositorio<Empleado> _empleados;

    public BusquedaEmpleadoLogica(IRepositorio<Persona> personas, IRepositorio<Empleado> empleados)
    {
        _personas = personas;
        _empleados = empleados;
    }

    /// <summary>Id de los empleados cuyo documento contiene los dígitos escritos; vacío si el texto no es un documento.</summary>
    public async Task<List<int>> PorDocumento(string? texto)
    {
        if (ReglasBusquedaEmpleado.NormalizarDocumento(texto) is not { } numero)
            return [];

        var personas = await _personas.ListarPagina(
            p => p.NumDocID != null && p.NumDocID.Contains(numero),
            q => q.OrderBy(p => p.NumDocID), 1, ReglasBusquedaEmpleado.MaximoResultados);
        var idsPersona = personas.Registros.Select(p => p.IdPersona).ToList();
        if (idsPersona.Count == 0)
            return [];

        var empleados = await _empleados.ListarPagina(
            e => e.IdPersona != null && idsPersona.Contains(e.IdPersona.Value),
            q => q.OrderBy(e => e.IdEmpleado), 1, ReglasBusquedaEmpleado.MaximoResultados);
        return empleados.Registros.Select(e => e.IdEmpleado).ToList();
    }
}
