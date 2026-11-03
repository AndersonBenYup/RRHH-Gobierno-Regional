using Capa_Datos;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Proyecto_Evo_RRLL.Salud;

/// <summary>SC-005: /estado responde "Healthy" solo si la app llega a SQL Server (para el monitoreo de Azure).</summary>
public sealed class BaseDatosHealthCheck : IHealthCheck
{
    private readonly EvoRRLDbContext _contexto;

    public BaseDatosHealthCheck(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _contexto.Database.CanConnectAsync(cancellationToken)
                ? HealthCheckResult.Healthy()
                : HealthCheckResult.Unhealthy("Sin conexión a la base de datos.");
        }
        catch (Exception ex)
        {
            // El detalle va al log; la respuesta pública no muestra datos internos.
            return HealthCheckResult.Unhealthy("Sin conexión a la base de datos.", ex);
        }
    }
}
