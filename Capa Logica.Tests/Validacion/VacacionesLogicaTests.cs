using System.Linq.Expressions;
using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica.Tests.Validacion;

/// <summary>RF-07 a RF-12: el reporte une el último periodo laboral con los permisos de vacaciones.</summary>
public class VacacionesLogicaTests
{
    private const int Vacaciones = 10;
    private const int Licencia = 20;
    private static readonly DateOnly Corte = new(2026, 11, 10);

    private static readonly Motivo_Perm[] Motivos =
    [
        new() { idMotivo = Vacaciones, Descrip_Motivo = "VACACIONES" },
        new() { idMotivo = Licencia, Descrip_Motivo = "LICENCIA SIN GOCE" }
    ];

    private static (VacacionesLogica Logica, PermisosEnMemoria Permisos) Crear(PeriodoLaboral[] periodos, params Permiso[] permisos)
    {
        var repo = new PermisosEnMemoria(permisos);
        return (new VacacionesLogica(repo, new EnMemoria<PeriodoLaboral>(periodos), new EnMemoria<Motivo_Perm>(Motivos)), repo);
    }

    private static PeriodoLaboral Periodo(int idEmpleado, int nro, DateTime? ingreso, DateTime? salida = null)
        => new() { IdEmpleado = idEmpleado, NroPeriodo = nro, FechaIngreso = ingreso, FechaSalida = salida };

    [Fact]
    public async Task Usa_el_ultimo_periodo_y_solo_descuenta_motivos_de_vacaciones()
    {
        var (logica, _) = Crear(
            [Periodo(1, 1, new DateTime(2010, 1, 1), new DateTime(2015, 1, 1)), Periodo(1, 2, new DateTime(2024, 1, 1))],
            new Permiso { IdEmpleado = 1, NPermiso = 1, idMotivo = Vacaciones, FechaInicio = new DateTime(2025, 2, 1), FechaFin = new DateTime(2025, 2, 10) },
            new Permiso { IdEmpleado = 1, NPermiso = 2, idMotivo = Licencia, FechaInicio = new DateTime(2025, 3, 1), FechaFin = new DateTime(2025, 3, 5) });

        var fila = Assert.Single(await logica.Reporte(Corte));

        Assert.Equal(new DateOnly(2024, 1, 1), fila.Resumen.FechaIngreso);
        Assert.Equal(60, fila.Resumen.Generadas);
        Assert.Equal(10, fila.Resumen.Gozadas);
        Assert.Equal(50, fila.Resumen.Pendientes);
        Assert.Single(fila.Goces);
    }

    [Fact]
    public async Task Los_cesados_solo_aparecen_si_se_piden_y_se_calculan_a_su_fecha_de_salida()
    {
        var (logica, _) = Crear([Periodo(2, 1, new DateTime(2025, 1, 1), new DateTime(2026, 3, 31))]);

        Assert.Empty(await logica.Reporte(Corte));

        var fila = Assert.Single(await logica.Reporte(Corte, incluirCesados: true));
        Assert.True(fila.Cesado);
        Assert.Equal(new DateOnly(2026, 3, 31), fila.Resumen.FechaCorte);
        Assert.Equal(30, fila.Resumen.Generadas);
        Assert.Equal(7.5m, fila.Resumen.Truncas);
    }

    [Fact]
    public async Task Sin_fecha_de_ingreso_no_se_puede_calcular()
    {
        var (logica, _) = Crear([Periodo(3, 1, null)]);

        Assert.Null(await logica.DeEmpleado(3, Corte));
    }

    // CP-RF10-02: unas vacaciones de 2 horas no llegan a media jornada; una licencia de 2 horas sí se acepta.
    [Theory]
    [InlineData(Vacaciones, 1)]
    [InlineData(Licencia, 0)]
    public async Task Las_vacaciones_por_horas_menores_a_media_jornada_se_rechazan(int motivo, int errores)
    {
        var permisos = new PermisosEnMemoria([]);
        var logica = new PermisoLogica(permisos, new EnMemoria<Motivo_Perm>(Motivos));
        var permiso = new Permiso
        {
            IdEmpleado = 1, idMotivo = motivo, FechaInicio = new DateTime(2026, 11, 10),
            Hora_Sal = new TimeOnly(8, 0), Hora_Ret = new TimeOnly(10, 0)
        };

        Assert.Equal(errores, (await logica.Validar(permiso)).Count);
    }

    private class EnMemoria<T> : IRepositorio<T> where T : class
    {
        protected readonly List<T> Datos;

        public EnMemoria(IEnumerable<T> datos) => Datos = datos.ToList();

        public Task<List<T>> Listar() => Task.FromResult(Datos.ToList());

        public Task<Pagina<T>> ListarPagina(Expression<Func<T, bool>>? filtro, Func<IQueryable<T>, IOrderedQueryable<T>> ordenar,
            int pagina, int tamanoPagina)
        {
            var consulta = Datos.AsQueryable();
            if (filtro is not null)
                consulta = consulta.Where(filtro);
            var lista = ordenar(consulta).ToList();
            return Task.FromResult(new Pagina<T>(lista.Skip((pagina - 1) * tamanoPagina).Take(tamanoPagina).ToList(), lista.Count, pagina, tamanoPagina));
        }

        public Task<T?> Obtener(params object[] claves)
            => Task.FromResult(Datos.FirstOrDefault(d => d is Motivo_Perm m && claves.Length == 1 && m.idMotivo.Equals(claves[0])));

        public Task Crear(T registro) { Datos.Add(registro); return Task.CompletedTask; }
        public Task Actualizar(T registro) => Task.CompletedTask;
        public Task Eliminar(T registro) { Datos.Remove(registro); return Task.CompletedTask; }
    }

    private sealed class PermisosEnMemoria : EnMemoria<Permiso>, IPermisoRepositorio
    {
        public PermisosEnMemoria(IEnumerable<Permiso> datos) : base(datos) { }

        public Task<List<Permiso>> ListarPorEmpleado(int idEmpleado)
            => Task.FromResult(Datos.Where(p => p.IdEmpleado == idEmpleado).ToList());

        public Task<Pagina<Permiso>> ListarPaginado(int? idEmpleado, int pagina, int tamanoPagina) => throw new NotSupportedException();

        public Task<int> SiguienteNumero(int idEmpleado) => Task.FromResult(Datos.Count + 1);

        public Task<List<Permiso>> ListarPorMotivosEntreFechas(IReadOnlyCollection<int> motivos, DateTime desde, DateTime hasta)
            => throw new NotSupportedException();

        public Task<List<Permiso>> ListarPorMotivos(IReadOnlyCollection<int> motivos, int? idEmpleado)
            => Task.FromResult(Datos.Where(p => p.idMotivo is int m && motivos.Contains(m)
                && (idEmpleado == null || p.IdEmpleado == idEmpleado)).ToList());
    }
}
