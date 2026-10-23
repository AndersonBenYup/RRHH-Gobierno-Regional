using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Capa_Datos;

public class SecuenciaService
{
    private readonly EvoRRLDbContext _contexto;

    public SecuenciaService(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<bool> EjecutarConBloqueoAsync(
        Func<Task<int>> calcularSiguiente,
        Func<int, Task<bool>> guardar,
        CancellationToken ct = default)
    {
        var guardo = await EjecutarTransaccionalAsync(
            async () =>
            {
                if (!await guardar(await calcularSiguiente()))
                    throw new TransaccionAbortadaException();
            }, ct);

        return guardo;
    }

    public async Task<bool> EjecutarTransaccionalAsync(
        Func<Task> trabajo,
        CancellationToken ct = default)
    {
        for (var intento = 1; intento <= 5; intento++)
        {
            try
            {
                await using var transaccion = await _contexto.Database
                    .BeginTransactionAsync(IsolationLevel.Serializable, ct);
                await trabajo();
                await transaccion.CommitAsync(ct);
                return true;
            }
            catch (TransaccionAbortadaException)
            {
                return false;
            }
            catch (Exception ex) when (EsTransitorio(ex) && intento < 5)
            {
                _contexto.ChangeTracker.Clear();
            }
        }
        return false;
    }

    private static bool EsTransitorio(Exception ex)
    {
        for (var actual = ex; actual is not null; actual = actual.InnerException)
        {
            if (actual is SqlException { } sql && (sql.Number is 1204 or 1205 or 1222 or 2601 or 2627))
                return true;
        }
        return false;
    }
}

internal sealed class TransaccionAbortadaException : Exception
{
}