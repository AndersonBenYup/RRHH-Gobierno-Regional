using Capa_Entidades;
using Capa_Entidades.Contratos;
using Capa_Logica.Reglas;

namespace Capa_Logica;

/// <summary>Un permiso de vacaciones con los días que descuenta del saldo.</summary>
public sealed record GoceDetalle(Permiso Permiso, GoceVacaciones Goce, decimal Dias);

/// <summary>Vacaciones de un empleado: su saldo y los permisos de vacaciones que lo explican.</summary>
public sealed record VacacionesEmpleado(int IdEmpleado, bool Cesado, ResumenVacaciones Resumen, IReadOnlyList<GoceDetalle> Goces);

/// <summary>Resultado del reporte: los empleados calculados y cuántos se omitieron por no tener fecha de ingreso vigente.</summary>
public sealed record ReporteVacaciones(IReadOnlyList<VacacionesEmpleado> Empleados, int SinFechaDeIngreso);

/// <summary>Fechas de servicio con las que se calcula: ingreso y, si cesó, salida.</summary>
public sealed record ServicioEmpleado(DateOnly Ingreso, DateOnly? Salida);

/// <summary>
/// RF-07 a RF-12: reporte de vacaciones generadas, gozadas, pendientes y truncas.
/// El ingreso sale de <see cref="FechasDeServicio"/> y los goces de los permisos cuyo motivo es de vacaciones.
/// </summary>
public class VacacionesLogica
{
    private readonly IPermisoRepositorio _permisos;
    private readonly IRepositorio<Empleado> _empleados;
    private readonly IRepositorio<PeriodoLaboral> _periodos;
    private readonly IRepositorio<Motivo_Perm> _motivos;

    public VacacionesLogica(IPermisoRepositorio permisos, IRepositorio<Empleado> empleados,
        IRepositorio<PeriodoLaboral> periodos, IRepositorio<Motivo_Perm> motivos)
    {
        _permisos = permisos;
        _empleados = empleados;
        _periodos = periodos;
        _motivos = motivos;
    }

    /// <summary>Motivos de permiso que son vacaciones (por su descripción).</summary>
    public async Task<List<Motivo_Perm>> MotivosDeVacaciones()
        => (await _motivos.Listar()).Where(m => CalculoVacaciones.EsMotivoDeVacaciones(m.Descrip_Motivo)).ToList();

    /// <summary>Saldo de cada empleado a la fecha de corte (ver <see cref="Generar"/>).</summary>
    public async Task<List<VacacionesEmpleado>> Reporte(DateOnly corte, int? idEmpleado = null, bool incluirCesados = false)
        => (await Generar(corte, idEmpleado, incluirCesados)).Empleados.ToList();

    /// <summary>
    /// Saldo de cada empleado a la fecha de corte. Para un cesado el corte es su fecha de salida.
    /// Los empleados sin fecha de ingreso vigente no se pueden calcular: se cuentan en <see cref="ReporteVacaciones.SinFechaDeIngreso"/>.
    /// </summary>
    public async Task<ReporteVacaciones> Generar(DateOnly corte, int? idEmpleado = null, bool incluirCesados = false)
    {
        var motivos = await MotivosDeVacaciones();
        var mediaJornada = motivos.Where(m => CalculoVacaciones.EsMotivoDeMediaJornada(m.Descrip_Motivo))
            .Select(m => m.idMotivo).ToHashSet();

        List<Empleado> empleados;
        List<PeriodoLaboral> periodos;
        if (idEmpleado is int id)
        {
            empleados = await _empleados.Obtener(id) is { } empleado ? [empleado] : [];
            periodos = (await _periodos.ListarPagina(p => p.IdEmpleado == id, q => q.OrderByDescending(p => p.NroPeriodo), 1, 500)).Registros;
        }
        else
        {
            empleados = await _empleados.Listar();
            periodos = await _periodos.Listar();
        }

        var ultimos = UltimoPeriodoPorEmpleado(periodos).ToDictionary(p => p.IdEmpleado);
        var goces = (await _permisos.ListarPorMotivos(motivos.Select(m => m.idMotivo).ToList(), idEmpleado)).ToLookup(p => p.IdEmpleado);

        var resultado = new List<VacacionesEmpleado>();
        var sinFecha = 0;
        foreach (var empleado in empleados)
        {
            if (FechasDeServicio(empleado, ultimos.GetValueOrDefault(empleado.IdEmpleado), corte) is not { } servicio)
            {
                sinFecha++;
                continue;
            }

            var cesado = servicio.Salida is { } salida && salida <= corte;
            if (cesado && !incluirCesados)
                continue;

            var ingreso = servicio.Ingreso;
            var corteEfectivo = cesado ? servicio.Salida!.Value : corte;
            if (corteEfectivo < ingreso)
                continue;   // ingresa después del corte

            var detalle = goces[empleado.IdEmpleado]
                .Select(p => GoceVacaciones.Desde(p, p.idMotivo is int m && mediaJornada.Contains(m)) is { } g
                    ? new GoceDetalle(p, g, CalculoVacaciones.DiasGozados(g, ingreso, corteEfectivo))
                    : null)
                .OfType<GoceDetalle>()
                .Where(d => d.Dias > 0)
                .OrderByDescending(d => d.Permiso.FechaInicio)
                .ToList();
            var resumen = CalculoVacaciones.Calcular(ingreso, corteEfectivo, detalle.Select(d => d.Goce));
            resultado.Add(new VacacionesEmpleado(empleado.IdEmpleado, cesado, resumen, detalle));
        }

        return new ReporteVacaciones(resultado, sinFecha);
    }

    /// <summary>RF-12: vacaciones de un empleado (incluye cesados); null si no tiene fecha de ingreso vigente.</summary>
    public async Task<VacacionesEmpleado?> DeEmpleado(int idEmpleado, DateOnly corte)
        => (await Reporte(corte, idEmpleado, incluirCesados: true)).FirstOrDefault();

    /// <summary>Un empleado puede tener varios periodos laborales; se usa el último con fecha de ingreso.</summary>
    public static IEnumerable<PeriodoLaboral> UltimoPeriodoPorEmpleado(IEnumerable<PeriodoLaboral> periodos)
        => periodos.Where(p => p.FechaIngreso is not null)
            .GroupBy(p => p.IdEmpleado)
            .Select(g => g.OrderByDescending(p => p.NroPeriodo).First());

    /// <summary>
    /// Fechas de servicio de un empleado. En la BD solo 833 de 3338 empleados tienen periodo laboral, y muchos activos
    /// tienen su último periodo cerrado hace años (datos migrados); por eso:
    /// <list type="bullet">
    /// <item>Activo (o sin estado): el último periodo si sigue abierto al corte; si no, <c>Empleado.FechaIngreso</c>.
    /// Nunca se le trata como cesado por un periodo antiguo cerrado.</item>
    /// <item>Inactivo: el último periodo cerrado (ingreso y salida); si el periodo no tiene ingreso no existe y se usa
    /// <c>Empleado.FechaIngreso</c> con esa salida. Sin fecha de salida no se pueden calcular sus truncas.</item>
    /// </list>
    /// Devuelve null si no hay fechas suficientes.
    /// </summary>
    public static ServicioEmpleado? FechasDeServicio(Empleado empleado, PeriodoLaboral? ultimoPeriodo, DateOnly corte)
    {
        var ingresoEmpleado = Fecha(empleado.FechaIngreso);
        var ingresoPeriodo = Fecha(ultimoPeriodo?.FechaIngreso);
        var salidaPeriodo = Fecha(ultimoPeriodo?.FechaSalida);

        if (empleado.Estado != false)
        {
            if (ingresoPeriodo is { } abierto && (salidaPeriodo is null || salidaPeriodo > corte))
                return new ServicioEmpleado(abierto, salidaPeriodo);
            return ingresoEmpleado is { } ingreso ? new ServicioEmpleado(ingreso, null) : null;
        }

        var ingresoCesado = ingresoPeriodo ?? ingresoEmpleado;
        if (ingresoCesado is null || salidaPeriodo is null || salidaPeriodo < ingresoCesado)
            return null;
        return new ServicioEmpleado(ingresoCesado.Value, salidaPeriodo);
    }

    private static DateOnly? Fecha(DateTime? fecha) => fecha is DateTime f ? DateOnly.FromDateTime(f) : null;
}
