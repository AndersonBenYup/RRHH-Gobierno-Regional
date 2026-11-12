using Capa_Entidades;
using Capa_Entidades.Contratos;
using Capa_Entidades.Validacion;
using Capa_Logica.Reglas;

namespace Capa_Logica;

public class PermisoLogica : LogicaBase<Permiso>
{
    /// <summary>SC-005: tamaño de página del listado de permisos.</summary>
    public const int TamanoPagina = 25;

    private readonly IPermisoRepositorio _permisos;
    private readonly IRepositorio<Motivo_Perm> _motivos;

    public PermisoLogica(IPermisoRepositorio permisos, IRepositorio<Motivo_Perm> motivos) : base(permisos)
    {
        _permisos = permisos;
        _motivos = motivos;
    }

    public Task<Permiso?> Obtener(int idEmpleado, int nPermiso) => Repositorio.Obtener(idEmpleado, nPermiso);

    /// <summary>SC-005: filtro por empleado y paginación resueltos en la BD.</summary>
    public Task<Pagina<Permiso>> ListarPaginado(int? idEmpleado, int pagina, int tamanoPagina = TamanoPagina)
        => _permisos.ListarPaginado(idEmpleado, pagina, tamanoPagina);

    /// <summary>SC-005: siguiente número correlativo del empleado calculado en la BD.</summary>
    public Task<int> SiguienteNumero(int idEmpleado) => _permisos.SiguienteNumero(idEmpleado);

    private async Task<bool> EsVacaciones(int? idMotivo)
        => idMotivo is int id && CalculoVacaciones.EsMotivoDeVacaciones((await _motivos.Obtener(id))?.Descrip_Motivo);

    /// <summary>RF-18 / SC-005: ausencias de los motivos elegidos que se cruzan con el rango, filtradas en la BD.</summary>
    public Task<List<Permiso>> ListarAusencias(IReadOnlyCollection<int> motivos, DateOnly desde, DateOnly hasta)
        => _permisos.ListarPorMotivosEntreFechas(motivos, desde.ToDateTime(TimeOnly.MinValue), hasta.ToDateTime(TimeOnly.MinValue));

    /// <summary>SC-008: reglas de negocio (fechas, horas y solapamiento) que se revisan antes de guardar.</summary>
    public async Task<List<ErrorValidacion>> Validar(Permiso permiso)
    {
        var registrados = await _permisos.ListarPorEmpleado(permiso.IdEmpleado);
        var errores = ReglasPermiso.Validar(permiso, registrados);
        if (errores.Count == 0 && ReglaLicenciaAnual.TieneOtraLicenciaDelMismoAnio(permiso, registrados))
            errores.Add(new(nameof(Permiso.idMotivo), "El empleado ya tiene registrada una licencia de este grupo en el año de inicio."));

        // RF-10: las vacaciones se cuentan desde media jornada; por horas, al menos 4 horas.
        if (errores.Count == 0 && !CalculoVacaciones.CumpleMediaJornada(permiso) && await EsVacaciones(permiso.idMotivo))
            errores.Add(new(nameof(Permiso.Hora_Ret), CalculoVacaciones.MensajeMediaJornada));

        return errores;
    }
}
