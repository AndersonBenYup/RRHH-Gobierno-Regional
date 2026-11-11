using Capa_Datos;
using Capa_Datos.Repositorios;
using Capa_Entidades;
using Microsoft.EntityFrameworkCore;

namespace Capa_Logica.Tests.Datos;

/// <summary>
/// SC-005: los reportes de ausencias, teletrabajo y vacaciones deben filtrarse en SQL Server.
/// ToQueryString() falla si EF no puede traducir la consulta (no necesita conexión a la BD).
/// </summary>
public class ConsultasEnBdTests
{
    private static EvoRRLDbContext Contexto()
        => new(new DbContextOptionsBuilder<EvoRRLDbContext>()
            .UseSqlServer("Server=.;Database=NoSeUsa;Trusted_Connection=True")
            .Options);

    [Fact]
    public void Ausencias_por_motivos_y_fechas_se_filtran_en_la_BD()
    {
        using var db = Contexto();
        var sql = PermisoRepositorio.ConsultaPorMotivosEntreFechas(db.Set<Permiso>(), [3, 7],
            new DateTime(2026, 11, 10), new DateTime(2026, 11, 15)).ToQueryString();

        Assert.Contains("WHERE", sql);
        Assert.Contains("[idMotivo]", sql);
        Assert.Contains("COALESCE([p].[FechaFin], [p].[FechaInicio])", sql);
    }

    [Fact]
    public void Permisos_por_motivos_de_un_empleado_se_filtran_en_la_BD()
    {
        using var db = Contexto();
        var sql = PermisoRepositorio.ConsultaPorMotivos(db.Set<Permiso>(), [3], 25).ToQueryString();

        Assert.Contains("WHERE", sql);
        Assert.Contains("[IdEmpleado]", sql);
    }

    [Fact]
    public void Periodos_de_teletrabajo_se_filtran_en_la_BD()
    {
        using var db = Contexto();
        var sql = PeriodoTeletrabajoRepositorio.ConsultaEntreFechas(db.Set<PeriodoTeletrabajo>(), 25, true,
            new DateOnly(2026, 11, 10), new DateOnly(2026, 11, 15)).ToQueryString();

        Assert.Contains("WHERE", sql);
        Assert.Contains("[Parcial]", sql);
        Assert.Contains("[FechaInicio]", sql);
    }
}
