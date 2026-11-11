using Capa_Entidades;
using Capa_Logica.Reglas;

namespace Capa_Logica.Tests.Validacion;

/// <summary>RF-07 a RF-10: 30 días por año completo de servicio, truncas proporcionales y media jornada.</summary>
public class CalculoVacacionesTests
{
    private static readonly DateOnly Ingreso = new(2024, 3, 1);

    // CP-RF07-01: el año se cumple el día anterior al aniversario (ambas fechas incluidas).
    [Theory]
    [InlineData(2025, 2, 27, 0)]
    [InlineData(2025, 2, 28, 1)]
    [InlineData(2026, 2, 28, 2)]
    [InlineData(2024, 2, 1, 0)]
    public void Cuenta_los_anios_completos_de_servicio(int anio, int mes, int dia, int esperado)
        => Assert.Equal(esperado, CalculoVacaciones.AniosCumplidos(Ingreso, new DateOnly(anio, mes, dia)));

    [Fact]
    public void Genera_30_dias_por_anio_completo()
    {
        var resumen = CalculoVacaciones.Calcular(Ingreso, new DateOnly(2026, 3, 10), []);

        Assert.Equal(2, resumen.AniosCumplidos);
        Assert.Equal(60, resumen.Generadas);
        Assert.Equal(60, resumen.Pendientes);
    }

    // CP-RF09-01: 2 meses completos (2,5 días cada uno) y 15 días (1,25).
    [Fact]
    public void Las_truncas_son_proporcionales_al_anio_en_curso()
        => Assert.Equal(6.25m, CalculoVacaciones.Truncas(Ingreso, new DateOnly(2026, 5, 15)));

    [Fact]
    public void Al_cumplir_el_anio_las_truncas_vuelven_a_cero()
        => Assert.Equal(0m, CalculoVacaciones.Truncas(Ingreso, new DateOnly(2026, 2, 28)));

    [Fact]
    public void Un_anio_sin_completar_genera_solo_truncas()
    {
        var resumen = CalculoVacaciones.Calcular(new DateOnly(2026, 1, 1), new DateOnly(2026, 6, 30), []);

        Assert.Equal(0, resumen.Generadas);
        Assert.Equal(15m, resumen.Truncas);
    }

    // CP-RF08-01 y CP-RF10-01: 10 días completos y una media jornada.
    [Fact]
    public void Las_pendientes_descuentan_dias_completos_y_medias_jornadas()
    {
        GoceVacaciones[] goces =
        [
            new(new DateOnly(2025, 7, 1), new DateOnly(2025, 7, 10), PorHoras: false),
            new(new DateOnly(2025, 8, 4), new DateOnly(2025, 8, 4), PorHoras: true)
        ];

        var resumen = CalculoVacaciones.Calcular(Ingreso, new DateOnly(2025, 12, 31), goces);

        Assert.Equal(10.5m, resumen.Gozadas);
        Assert.Equal(19.5m, resumen.Pendientes);
    }

    [Fact]
    public void Solo_cuentan_los_goces_entre_el_ingreso_y_el_corte()
    {
        GoceVacaciones[] goces =
        [
            new(new DateOnly(2024, 2, 25), new DateOnly(2024, 3, 2), false),   // 2 días después del ingreso
            new(new DateOnly(2026, 1, 1), new DateOnly(2026, 1, 5), false)     // después del corte
        ];

        var resumen = CalculoVacaciones.Calcular(Ingreso, new DateOnly(2025, 12, 31), goces);

        Assert.Equal(2m, resumen.Gozadas);
    }

    [Fact]
    public void Las_vacaciones_adelantadas_dejan_pendientes_negativas()
    {
        var resumen = CalculoVacaciones.Calcular(new DateOnly(2026, 1, 1), new DateOnly(2026, 6, 30),
            [new(new DateOnly(2026, 6, 1), new DateOnly(2026, 6, 5), false)]);

        Assert.Equal(-5m, resumen.Pendientes);
    }

    [Theory]
    [InlineData("08:00", "12:00", true)]
    [InlineData("08:00", "11:59", false)]
    [InlineData("13:00", "17:30", true)]
    public void Las_vacaciones_por_horas_exigen_media_jornada(string salida, string retorno, bool esperado)
    {
        var permiso = new Permiso
        {
            FechaInicio = new DateTime(2026, 11, 10),
            Hora_Sal = TimeOnly.Parse(salida),
            Hora_Ret = TimeOnly.Parse(retorno)
        };

        Assert.Equal(esperado, CalculoVacaciones.CumpleMediaJornada(permiso));
    }

    [Fact]
    public void Un_permiso_de_dia_completo_no_necesita_horas()
        => Assert.True(CalculoVacaciones.CumpleMediaJornada(new Permiso { Dia = true, FechaInicio = new DateTime(2026, 11, 10) }));

    [Fact]
    public void Un_permiso_por_horas_se_convierte_en_media_jornada()
    {
        var goce = GoceVacaciones.Desde(new Permiso
        {
            FechaInicio = new DateTime(2026, 11, 10),
            Hora_Sal = new TimeOnly(8, 0),
            Hora_Ret = new TimeOnly(12, 0)
        })!;

        Assert.True(goce.PorHoras);
        Assert.Equal(0.5m, CalculoVacaciones.DiasGozados(goce, DateOnly.MinValue, DateOnly.MaxValue));
    }

    [Theory]
    [InlineData("VACACIONES", true)]
    [InlineData("Vacación por adelanto", true)]
    [InlineData("VACACIONES TRUNCAS", true)]
    [InlineData("LICENCIA POR ENFERMEDAD", false)]
    [InlineData(null, false)]
    public void Reconoce_los_motivos_de_vacaciones_por_su_descripcion(string? descripcion, bool esperado)
        => Assert.Equal(esperado, CalculoVacaciones.EsMotivoDeVacaciones(descripcion));
}
