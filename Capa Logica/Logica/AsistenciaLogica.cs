using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class AsistenciaLogica : LogicaBase<Asistencia>
{
    private readonly IAsistenciaRepositorio _asistencias;

    public AsistenciaLogica(IAsistenciaRepositorio asistencias) : base(asistencias) => _asistencias = asistencias;

    public Task<Asistencia?> Obtener(DateTime fecha, int idEmpleado) => Repositorio.Obtener(fecha, idEmpleado);
    public Task<int> ContarEntreFechas(DateTime desde, DateTime hasta) => _asistencias.ContarEntreFechas(desde, hasta);

    /// <summary>SC-003: asistencias de un rango, filtradas en la BD (antes se traía toda la tabla).</summary>
    public Task<List<Asistencia>> ListarEntreFechas(DateTime desde, DateTime hastaExclusivo)
        => _asistencias.ListarEntreFechas(desde, hastaExclusivo);
}
