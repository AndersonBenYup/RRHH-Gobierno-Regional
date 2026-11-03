using Capa_Logica.Seguridad;

namespace Capa_Logica.Tests.Seguridad;

/// <summary>SC-009: permisos por opción (Rol_Acceso).</summary>
public class ReglasAccesoTests
{
    private const string Personal = "0502001";
    private const string Permisos = "0502007";
    private const string Asistencia = "0502004";

    private static (int, string?, string?) Rol(int id, string descripcion, string sistema) => (id, descripcion, sistema);
    private static (int, string?) Acceso(int idRol, string opcion) => (idRol, opcion);

    [Theory]
    [InlineData("Administrador", "0400000", true)]   // rol creado por el sistema
    [InlineData("ADMINISTRADOR", "0500000", true)]   // administrador de RRHH
    [InlineData(" administrador ", "0500000", true)]
    [InlineData("Administrador", "GRLL", true)]
    [InlineData("ADMINISTRADOR", "0600000", false)]  // Consejo Regional
    [InlineData("ADMINISTRADOR", "0200000", false)]  // SIA
    [InlineData("ADMINISTRADOR", "0800000", false)]  // SIAB
    [InlineData("ASISTENCIAS", "0500000", false)]
    [InlineData(null, null, false)]
    public void Solo_los_administradores_de_RRHH_lo_son(string? descripcion, string? sistema, bool esperado)
        => Assert.Equal(esperado, ReglasAcceso.EsAdministradorRrhh(descripcion, sistema));

    [Fact]
    public void El_administrador_ve_todo_aunque_tenga_accesos_marcados()
    {
        var acceso = ReglasAcceso.Calcular([Rol(13, "ADMINISTRADOR", "0500000")], [Acceso(13, Personal)]);
        Assert.True(acceso.EsAdministrador);
        Assert.True(acceso.Puede(Permisos));
    }

    [Fact]
    public void Un_rol_sin_modulos_configurados_ve_todo()
    {
        var acceso = ReglasAcceso.Calcular([Rol(10, "ASISTENCIAS", "0500000")], []);
        Assert.False(acceso.EsAdministrador);
        Assert.True(acceso.VeTodo);
    }

    [Fact]
    public void Un_rol_configurado_ve_solo_sus_modulos()
    {
        var acceso = ReglasAcceso.Calcular([Rol(10, "ASISTENCIAS", "0500000")], [Acceso(10, Asistencia)]);
        Assert.True(acceso.Puede(Asistencia));
        Assert.False(acceso.Puede(Personal));
    }

    [Fact]
    public void Con_varios_roles_configurados_se_suman_los_modulos()
    {
        var acceso = ReglasAcceso.Calcular(
            [Rol(10, "ASISTENCIAS", "0500000"), Rol(4, "REGISTRAR PLANILLA", "0500000")],
            [Acceso(10, Asistencia), Acceso(4, Personal)]);
        Assert.True(acceso.Puede(Asistencia));
        Assert.True(acceso.Puede(Personal));
        Assert.False(acceso.Puede(Permisos));
    }

    [Fact]
    public void Si_uno_de_sus_roles_no_esta_configurado_ve_todo()
    {
        var acceso = ReglasAcceso.Calcular(
            [Rol(10, "ASISTENCIAS", "0500000"), Rol(8, "COMPRAS", "0800000")],
            [Acceso(10, Asistencia)]);
        Assert.True(acceso.VeTodo);
    }

    [Fact]
    public void Las_opciones_de_otros_sistemas_no_cuentan_como_configuracion()
    {
        var acceso = ReglasAcceso.Calcular([Rol(8, "COMPRAS", "0800000")], [Acceso(8, "0800000")]);
        Assert.True(acceso.VeTodo);
    }

    [Theory]
    [InlineData("Empleado", "0502001")]
    [InlineData("familiasalud", "0502003")]
    [InlineData("Catalogo", "0502012")]
    [InlineData("Home", null)]
    [InlineData("SeguridadAdmin", null)]
    public void Cada_controller_pertenece_a_un_modulo(string controller, string? codigo)
        => Assert.Equal(codigo, ReglasAcceso.ModuloDe(controller)?.Codigo);

    [Fact]
    public void Los_codigos_de_modulo_son_unicos_y_de_siete_caracteres()
    {
        Assert.Equal(ReglasAcceso.Modulos.Count, ReglasAcceso.Modulos.Select(m => m.Codigo).Distinct().Count());
        Assert.All(ReglasAcceso.Modulos, m => Assert.Equal(7, m.Codigo.Length));
    }
}
