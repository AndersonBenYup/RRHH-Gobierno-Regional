using System.ComponentModel.DataAnnotations;
using Capa_Entidades;
using Capa_Entidades.Contratos;
using Capa_Logica.Seguridad;

namespace Capa_Logica.Tests.Seguridad;

/// <summary>Los parámetros del bloqueo salen de la configuración (IOptions) y no del código.</summary>
public class OpcionesSeguridadTests
{
    private sealed class BitacoraEnMemoria : IBitacoraRepositorio
    {
        private readonly List<(string Usuario, string Accion, DateTime Fecha)> _eventos = [];
        private readonly Func<DateTime> _ahora;

        public BitacoraEnMemoria(Func<DateTime> ahora) => _ahora = ahora;

        public Task Registrar(string usuario, string accion, string entidad, string? detalle = null)
        {
            _eventos.Add((usuario, accion, _ahora()));
            return Task.CompletedTask;
        }

        public Task<List<Bitacora>> Ultimas(int cantidad) => Task.FromResult(new List<Bitacora>());

        public Task<List<DateTime>> Fechas(string usuario, string accion, DateTime desde)
            => Task.FromResult(_eventos.Where(e => e.Usuario == usuario && e.Accion == accion && e.Fecha >= desde)
                .Select(e => e.Fecha).ToList());

        public Task<DateTime?> UltimaFecha(string usuario, string[] acciones)
            => Task.FromResult(_eventos.Where(e => e.Usuario == usuario && acciones.Contains(e.Accion))
                .Select(e => (DateTime?)e.Fecha).DefaultIfEmpty().Max());
    }

    private sealed class RelojFijo(DateTimeOffset ahora) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => ahora;
        public override TimeZoneInfo LocalTimeZone => TimeZoneInfo.Utc;
    }

    private static (BloqueoCuentaLogica Bloqueo, BitacoraEnMemoria Bitacora) Crear(OpcionesSeguridad opciones)
    {
        var reloj = new RelojFijo(new DateTimeOffset(2026, 11, 2, 10, 0, 0, TimeSpan.Zero));
        var bitacora = new BitacoraEnMemoria(() => reloj.GetLocalNow().DateTime);
        return (new BloqueoCuentaLogica(bitacora, reloj, opciones), bitacora);
    }

    [Fact]
    public async Task Con_tres_intentos_configurados_la_cuenta_se_bloquea_al_tercero()
    {
        var (bloqueo, _) = Crear(new OpcionesSeguridad { MaxIntentosFallidos = 3 });

        Assert.False((await bloqueo.RegistrarFallo("ana", null)).Bloqueada);
        Assert.False((await bloqueo.RegistrarFallo("ana", null)).Bloqueada);
        Assert.True((await bloqueo.RegistrarFallo("ana", null)).Bloqueada);
        Assert.Equal(3, bloqueo.MaxIntentos);
    }

    [Fact]
    public async Task Por_defecto_se_mantienen_cinco_intentos()
    {
        var (bloqueo, _) = Crear(new OpcionesSeguridad());
        for (var i = 0; i < 4; i++)
            Assert.False((await bloqueo.RegistrarFallo("ana", null)).Bloqueada);
        Assert.True((await bloqueo.RegistrarFallo("ana", null)).Bloqueada);
    }

    [Theory]
    [InlineData(0, 15, false)]
    [InlineData(5, 0, false)]
    [InlineData(5, 15, true)]
    public void Valores_fuera_de_rango_no_pasan_la_validacion_al_arrancar(int intentos, int minutos, bool valido)
    {
        var opciones = new OpcionesSeguridad { MaxIntentosFallidos = intentos, MinutosBloqueo = minutos };
        var esValido = Validator.TryValidateObject(opciones, new ValidationContext(opciones), null, validateAllProperties: true);
        Assert.Equal(valido, esValido);
    }
}
