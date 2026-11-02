using Capa_Entidades;
using Capa_Entidades.Validacion;
using Capa_Logica.Reglas;

namespace Capa_Logica.Tests.Validacion;

/// <summary>SC-008: nombres, DNI de 8 dígitos y documento duplicado.</summary>
public class ReglasPersonaTests
{
    private static Persona Persona(string? doc = "12345678", int? tipo = TiposDocumento.Dni)
        => new() { Nombres = "Ana", Apellido_Paterno = "Ruiz", TipoDocID = tipo, NumDocID = doc };

    [Fact]
    public void Persona_valida_no_tiene_errores()
        => Assert.Empty(ReglasPersona.Validar(Persona(), documentoDuplicado: false));

    [Theory]
    [InlineData("1234567")]
    [InlineData("123456789")]
    [InlineData("1234567A")]
    [InlineData("ABCDEFGH")]
    public void Dni_que_no_tiene_8_digitos_es_error(string dni)
    {
        var error = Assert.Single(ReglasPersona.Validar(Persona(dni), false));
        Assert.Equal(nameof(Capa_Entidades.Persona.NumDocID), error.Campo);
        Assert.Equal(DniAttribute.Mensaje, error.Mensaje);
    }

    [Fact]
    public void Dni_con_espacios_de_relleno_es_valido()
        => Assert.Empty(ReglasPersona.Validar(Persona("12345678    "), false));

    [Fact]
    public void Ruc_debe_tener_11_digitos()
    {
        Assert.Empty(ReglasPersona.Validar(Persona("20123456789", TiposDocumento.Ruc), false));
        Assert.Single(ReglasPersona.Validar(Persona("2012345", TiposDocumento.Ruc), false));
    }

    [Fact]
    public void Partida_de_nacimiento_no_exige_formato()
        => Assert.Empty(ReglasPersona.Validar(Persona("PN-0001", TiposDocumento.PartidaNacimiento), false));

    [Fact]
    public void Documento_duplicado_es_error()
    {
        var error = Assert.Single(ReglasPersona.Validar(Persona(), documentoDuplicado: true));
        Assert.Equal(ReglasPersona.MensajeDuplicado, error.Mensaje);
    }

    [Fact]
    public void Tipo_de_documento_sin_numero_es_error()
    {
        var error = Assert.Single(ReglasPersona.Validar(Persona(doc: "  "), false));
        Assert.Equal(ReglasPersona.MensajeSinDocumento, error.Mensaje);
    }

    [Fact]
    public void Nombres_y_apellido_vacios_son_error()
    {
        var p = Persona();
        p.Nombres = " ";
        p.Apellido_Paterno = null;
        Assert.Equal(2, ReglasPersona.Validar(p, false).Count);
    }

    [Fact]
    public void Familiar_puede_no_tener_apellido_paterno()
    {
        var p = Persona();
        p.Apellido_Paterno = null;
        Assert.Empty(ReglasPersona.Validar(p, false, exigirApellido: false));
    }
}
