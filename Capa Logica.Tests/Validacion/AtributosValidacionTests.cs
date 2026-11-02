using System.ComponentModel.DataAnnotations;
using Capa_Entidades;
using Capa_Entidades.Validacion;

namespace Capa_Logica.Tests.Validacion;

/// <summary>SC-008: atributos [Dni], [FechaNoAnteriorA] y DataAnnotations de las entidades.</summary>
public class AtributosValidacionTests
{
    private static List<ValidationResult> Validar(object modelo)
    {
        var resultados = new List<ValidationResult>();
        Validator.TryValidateObject(modelo, new ValidationContext(modelo), resultados, validateAllProperties: true);
        return resultados;
    }

    [Theory]
    [InlineData("12345678", true)]
    [InlineData(null, true)]
    [InlineData("", true)]
    [InlineData("1234567", false)]
    [InlineData("12345678X", false)]
    public void Dni_acepta_solo_8_digitos(string? valor, bool esperado)
        => Assert.Equal(esperado, new DniAttribute().IsValid(valor));

    [Fact]
    public void Capacitacion_con_fin_anterior_al_inicio_es_invalida()
    {
        var c = new Capacitacion
        {
            IdEmpleado = 1,
            NombreCurso = "Excel",
            FechaInicio = new DateTime(2026, 5, 10),
            FechaFin = new DateTime(2026, 5, 1)
        };
        var error = Assert.Single(Validar(c));
        Assert.Contains(nameof(Capacitacion.FechaFin), error.MemberNames);
    }

    [Fact]
    public void Fechas_DateOnly_tambien_se_comparan()
    {
        var h = new HorarioTemporal
        {
            idEmpleado = 1,
            HorarioAsignado = 1,
            FechaInicio = new DateOnly(2026, 5, 10),
            FechaFin = new DateOnly(2026, 5, 9)
        };
        Assert.Contains(Validar(h), r => r.MemberNames.Contains(nameof(HorarioTemporal.FechaFin)));
    }

    [Fact]
    public void Permiso_sin_empleado_ni_motivo_es_invalido()
    {
        var p = new Permiso { IdEmpleado = 0, FechaInicio = DateTime.Today };
        var miembros = Validar(p).SelectMany(r => r.MemberNames).ToList();
        Assert.Contains(nameof(Permiso.IdEmpleado), miembros);
        Assert.Contains(nameof(Permiso.idMotivo), miembros);
    }

    [Fact]
    public void Texto_mas_largo_que_la_columna_es_invalido()
    {
        var f = new RRHH_Feriado { Fecha = DateTime.Today, Motivo = new string('x', 201) };
        var error = Assert.Single(Validar(f));
        Assert.Equal("Máximo 200 caracteres.", error.ErrorMessage);
    }

    [Fact]
    public void Feriado_completo_es_valido()
        => Assert.Empty(Validar(new RRHH_Feriado { Fecha = DateTime.Today, Motivo = "Día de todos los santos" }));
}
