using Capa_Logica.Seguridad;

namespace Capa_Logica.Tests.Seguridad;

/// <summary>SC-009 (completa SC-002): bloqueo de cuenta por intentos fallidos.</summary>
public class ReglasBloqueoTests
{
    private static readonly DateTime Ahora = new(2026, 11, 2, 10, 0, 0);

    private static IEnumerable<DateTime> Fallos(params int[] minutosAtras)
        => minutosAtras.Select(m => Ahora.AddMinutes(-m));

    [Fact]
    public void Sin_fallos_la_cuenta_esta_libre()
        => Assert.False(ReglasBloqueo.Evaluar([], null, Ahora).Bloqueada);

    [Fact]
    public void Cuatro_fallos_no_bloquean()
    {
        var estado = ReglasBloqueo.Evaluar(Fallos(4, 3, 2, 1), null, Ahora);
        Assert.False(estado.Bloqueada);
        Assert.Equal(4, estado.FallosRecientes);
    }

    [Fact]
    public void Quinto_fallo_bloquea_15_minutos_desde_el_ultimo()
    {
        var estado = ReglasBloqueo.Evaluar(Fallos(5, 4, 3, 2, 0), null, Ahora);
        Assert.True(estado.Bloqueada);
        Assert.Equal(Ahora.AddMinutes(15), estado.BloqueadaHasta);
        Assert.Equal(15, estado.MinutosRestantes(Ahora));
    }

    [Fact]
    public void El_bloqueo_expira_pasado_el_tiempo()
    {
        // Cinco fallos hace 16-20 minutos: fuera de la ventana.
        Assert.False(ReglasBloqueo.Evaluar(Fallos(20, 19, 18, 17, 16), null, Ahora).Bloqueada);
    }

    [Fact]
    public void Un_acceso_correcto_reinicia_el_conteo()
    {
        var estado = ReglasBloqueo.Evaluar(Fallos(10, 9, 8, 7, 1), ultimoReinicio: Ahora.AddMinutes(-5), Ahora);
        Assert.False(estado.Bloqueada);
        Assert.Equal(1, estado.FallosRecientes);
    }

    [Fact]
    public void El_desbloqueo_del_administrador_libera_la_cuenta()
    {
        var fallos = Fallos(5, 4, 3, 2, 1).ToList();
        Assert.True(ReglasBloqueo.Evaluar(fallos, null, Ahora).Bloqueada);
        Assert.False(ReglasBloqueo.Evaluar(fallos, ultimoReinicio: Ahora.AddSeconds(-10), Ahora).Bloqueada);
    }

    [Fact]
    public void Fallos_futuros_no_cuentan()
        => Assert.False(ReglasBloqueo.Evaluar(Fallos(-1, -2, -3, -4, -5), null, Ahora).Bloqueada);
}

/// <summary>SC-009: política de contraseñas.</summary>
public class PoliticaClaveTests
{
    [Theory]
    [InlineData("Rrhh2026")]
    [InlineData("clave segura 9")]
    [InlineData("Ñandú.2026")]
    public void Claves_que_cumplen_la_politica(string clave)
        => Assert.Empty(PoliticaClave.Validar(clave));

    [Theory]
    [InlineData("Ab1", "al menos 8")]
    [InlineData("soloLetras", "número")]
    [InlineData("12345678", "letra")]
    [InlineData("", "obligatoria")]
    [InlineData(null, "obligatoria")]
    public void Claves_que_no_cumplen(string? clave, string fragmento)
        => Assert.Contains(PoliticaClave.Validar(clave), e => e.Contains(fragmento));

    [Fact]
    public void La_clave_no_puede_contener_el_login()
        => Assert.Contains(PoliticaClave.Validar("Jperez2026", "jperez"), e => e.Contains("nombre de usuario"));
}
