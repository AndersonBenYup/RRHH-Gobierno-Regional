using Capa_Entidades;
using Capa_Logica.Reglas;

namespace Capa_Logica.Tests.Validacion;

/// <summary>SC-008 / SC-001: fechas, horas y solapamiento de permisos.</summary>
public class ReglasPermisoTests
{
    private static Permiso Permiso(int n, string inicio, string? fin = null, string? sal = null, string? ret = null, bool? dia = null)
        => new()
        {
            IdEmpleado = 7,
            NPermiso = n,
            idMotivo = 1,
            FechaInicio = DateTime.Parse(inicio),
            FechaFin = fin is null ? null : DateTime.Parse(fin),
            Hora_Sal = sal is null ? null : TimeOnly.Parse(sal),
            Hora_Ret = ret is null ? null : TimeOnly.Parse(ret),
            Dia = dia
        };

    [Fact]
    public void Permiso_valido_sin_otros_no_tiene_errores()
    {
        var errores = ReglasPermiso.Validar(Permiso(0, "2026-11-02", "2026-11-04"), []);
        Assert.Empty(errores);
    }

    [Fact]
    public void Fecha_fin_anterior_a_inicio_es_error()
    {
        var errores = ReglasPermiso.Validar(Permiso(0, "2026-11-05", "2026-11-02"), []);
        var error = Assert.Single(errores);
        Assert.Equal(nameof(Capa_Entidades.Permiso.FechaFin), error.Campo);
    }

    [Fact]
    public void Sin_fecha_de_inicio_es_error()
    {
        var p = Permiso(0, "2026-11-05");
        p.FechaInicio = null;
        var error = Assert.Single(ReglasPermiso.Validar(p, []));
        Assert.Equal(nameof(Capa_Entidades.Permiso.FechaInicio), error.Campo);
    }

    [Theory]
    [InlineData("10:00", "09:00")]
    [InlineData("10:00", "10:00")]
    public void Retorno_no_posterior_a_salida_el_mismo_dia_es_error(string sal, string ret)
    {
        var errores = ReglasPermiso.Validar(Permiso(0, "2026-11-02", "2026-11-02", sal, ret), []);
        var error = Assert.Single(errores);
        Assert.Equal(nameof(Capa_Entidades.Permiso.Hora_Ret), error.Campo);
    }

    [Fact]
    public void Horas_en_dias_distintos_no_se_comparan()
    {
        var errores = ReglasPermiso.Validar(Permiso(0, "2026-11-02", "2026-11-03", "15:00", "08:00"), []);
        Assert.Empty(errores);
    }

    [Fact]
    public void Se_cruza_con_otro_permiso_del_mismo_empleado()
    {
        var existente = Permiso(3, "2026-11-03", "2026-11-06");
        var errores = ReglasPermiso.Validar(Permiso(0, "2026-11-05", "2026-11-08"), [existente]);
        var error = Assert.Single(errores);
        Assert.Contains("N.º 3", error.Mensaje);
    }

    [Fact]
    public void Permisos_de_otro_empleado_no_cuentan()
    {
        var deOtro = Permiso(3, "2026-11-03", "2026-11-06");
        deOtro.IdEmpleado = 99;
        Assert.Empty(ReglasPermiso.Validar(Permiso(0, "2026-11-05"), [deOtro]));
    }

    [Fact]
    public void Al_editar_no_choca_consigo_mismo()
    {
        var guardado = Permiso(4, "2026-11-03", "2026-11-06");
        var editado = Permiso(4, "2026-11-03", "2026-11-07");
        Assert.Empty(ReglasPermiso.Validar(editado, [guardado]));
    }

    [Fact]
    public void Permisos_por_horas_del_mismo_dia_sin_cruce_de_horario_son_error()
    {
        var manana = Permiso(1, "2026-11-02", null, "08:00", "10:00");
        var tarde = Permiso(0, "2026-11-02", null, "15:00", "16:00");
        var error = Assert.Single(ReglasPermiso.Validar(tarde, [manana]));
        Assert.Contains("N.º 1", error.Mensaje);
    }

    [Fact]
    public void Permisos_por_horas_del_mismo_dia_con_cruce_de_horario_son_error()
    {
        var manana = Permiso(1, "2026-11-02", null, "08:00", "10:00");
        var otro = Permiso(0, "2026-11-02", null, "09:30", "11:00");
        Assert.Single(ReglasPermiso.Validar(otro, [manana]));
    }

    [Fact]
    public void Permiso_por_dia_choca_con_permiso_por_horas_del_mismo_dia()
    {
        var porHoras = Permiso(1, "2026-11-02", null, "08:00", "10:00");
        var diaCompleto = Permiso(0, "2026-11-02", dia: true);
        Assert.Single(ReglasPermiso.Validar(diaCompleto, [porHoras]));
    }

    [Fact]
    public void Dias_contiguos_no_se_solapan()
    {
        var antes = Permiso(1, "2026-11-01", "2026-11-02");
        Assert.Empty(ReglasPermiso.Validar(Permiso(0, "2026-11-03", "2026-11-04"), [antes]));
    }
}
