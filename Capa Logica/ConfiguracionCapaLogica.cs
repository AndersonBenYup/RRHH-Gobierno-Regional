using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Capa_Logica;

/// <summary>
/// SC-006: registra todas las clases de la Capa Lógica (las que terminan en "Logica") de una vez,
/// en lugar de los ~80 AddScoped que había en Program.cs.
/// </summary>
public static class ConfiguracionCapaLogica
{
    public static IServiceCollection AgregarCapaLogica(this IServiceCollection servicios)
    {
        var tipos = typeof(ConfiguracionCapaLogica).Assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false, IsGenericTypeDefinition: false, IsPublic: true }
                        && t.Name.EndsWith("Logica", StringComparison.Ordinal));
        foreach (var tipo in tipos)
            servicios.AddScoped(tipo);

        // Valores por defecto de seguridad; Program.cs los reemplaza con los de la configuración (IOptions).
        servicios.TryAddSingleton(new Seguridad.OpcionesSeguridad());

        // SC-003: exportadores (OCP). Un formato nuevo solo necesita otra línea aquí.
        servicios.AddSingleton<Exportacion.IExportadorTabla, Exportacion.ExportadorCsv>();
        servicios.AddSingleton<Exportacion.ExportadorTablas>();
        return servicios;
    }
}
