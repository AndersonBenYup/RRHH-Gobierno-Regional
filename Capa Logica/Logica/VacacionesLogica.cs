using Capa_Entidades;
using Capa_Entidades.Contratos;
using Capa_Logica.Reglas;

namespace Capa_Logica;

/// <summary>Vacaciones de un empleado: su saldo y los permisos de vacaciones que lo explican.</summary>
public sealed record VacacionesEmpleado(int IdEmpleado, bool Cesado, ResumenVacaciones Resumen, IReadOnlyList<Permiso> Goces);

/// <summary>
/// RF-07 a RF-12: reporte de vacaciones generadas, gozadas, pendientes y truncas.
/// El ingreso sale del último periodo laboral del empleado y los goces de los permisos cuyo motivo es de vacaciones.
/// </summary>
public class VacacionesLogica
{
    private readonly IPermisoRepositorio _permisos;
    private readonly IRepositorio<PeriodoLaboral> _periodos;
    private readonly IRepositorio<Motivo_Perm> _motivos;

    public VacacionesLogica(IPermisoRepositorio permisos, IRepositorio<PeriodoLaboral> periodos, IRepositorio<Motivo_Perm> motivos)
    {
        _permisos = permisos;
        _periodos = periodos;
        _motivos = motivos;
    }

    /// <summary>Motivos de permiso que son vacaciones (por su descripción).</summary>
    public async Task<List<Motivo_Perm>> MotivosDeVacaciones()
        => (await _motivos.Listar()).Where(m => CalculoVacaciones.EsMotivoDeVacaciones(m.Descrip_Motivo)).ToList();

    /// <summary>
    /// Saldo de cada empleado a la fecha de corte. Para un cesado el corte es su fecha de salida.
    /// Los empleados sin fecha de ingreso no se pueden calcular y no aparecen.
    /// </summary>
    public async Task<List<VacacionesEmpleado>> Reporte(DateOnly corte, int? idEmpleado = null, bool incluirCesados = false)
    {
        var motivos = (await MotivosDeVacaciones()).Select(m => m.idMotivo).ToList();
        var periodos = idEmpleado is int id
            ? (await _periodos.ListarPagina(p => p.IdEmpleado == id, q => q.OrderByDescending(p => p.NroPeriodo), 1, 500)).Registros
            : await _periodos.Listar();
        var goces = (await _permisos.ListarPorMotivos(motivos, idEmpleado)).ToLookup(p => p.IdEmpleado);

        var resultado = new List<VacacionesEmpleado>();
        foreach (var periodo in UltimoPeriodoPorEmpleado(periodos))
        {
            var ingreso = DateOnly.FromDateTime(periodo.FechaIngreso!.Value);
            var salida = periodo.FechaSalida is DateTime s ? DateOnly.FromDateTime(s) : (DateOnly?)null;
            var cesado = salida is not null && salida <= corte;
            if (cesado && !incluirCesados)
                continue;

            var corteEfectivo = cesado ? salida!.Value : corte;
            if (corteEfectivo < ingreso)
                continue;

            var permisos = goces[periodo.IdEmpleado].ToList();
            var resumen = CalculoVacaciones.Calcular(ingreso, corteEfectivo,
                permisos.Select(GoceVacaciones.Desde).OfType<GoceVacaciones>());
            resultado.Add(new VacacionesEmpleado(periodo.IdEmpleado, cesado, resumen,
                permisos.Where(p => GoceVacaciones.Desde(p) is { } g && CalculoVacaciones.DiasGozados(g, ingreso, corteEfectivo) > 0)
                    .OrderByDescending(p => p.FechaInicio).ToList()));
        }

        return resultado;
    }

    /// <summary>RF-12: vacaciones de un empleado (incluye cesados); null si no tiene fecha de ingreso.</summary>
    public async Task<VacacionesEmpleado?> DeEmpleado(int idEmpleado, DateOnly corte)
        => (await Reporte(corte, idEmpleado, incluirCesados: true)).FirstOrDefault();

    /// <summary>Un empleado puede tener varios periodos laborales; se usa el último con fecha de ingreso.</summary>
    public static IEnumerable<PeriodoLaboral> UltimoPeriodoPorEmpleado(IEnumerable<PeriodoLaboral> periodos)
        => periodos.Where(p => p.FechaIngreso is not null)
            .GroupBy(p => p.IdEmpleado)
            .Select(g => g.OrderByDescending(p => p.NroPeriodo).First());
}
