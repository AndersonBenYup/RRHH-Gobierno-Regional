namespace Capa_Entidades.Contratos;

/// <summary>Una página de resultados y el total de registros que cumplen el filtro.</summary>
public sealed record Pagina<T>(List<T> Registros, int Total, int NumeroPagina, int TamanoPagina)
{
    public int TotalPaginas => Math.Max(1, (int)Math.Ceiling(Total / (double)TamanoPagina));
}

/// <summary>SC-005 / SC-008: consultas de permisos que se resuelven en la BD.</summary>
public interface IPermisoRepositorio : IRepositorio<Permiso>
{
    Task<List<Permiso>> ListarPorEmpleado(int idEmpleado);

    /// <summary>Filtra, ordena (más recientes primero) y pagina en la BD con OFFSET/FETCH.</summary>
    Task<Pagina<Permiso>> ListarPaginado(int? idEmpleado, int pagina, int tamanoPagina);

    /// <summary>Siguiente NPermiso del empleado (MAX + 1) calculado en la BD.</summary>
    Task<int> SiguienteNumero(int idEmpleado);

    /// <summary>
    /// RF-18 / SC-005: permisos de los motivos indicados que se cruzan con el rango (límites incluidos).
    /// Un permiso sin fecha de fin dura solo su fecha de inicio. El filtro se ejecuta en la BD.
    /// </summary>
    Task<List<Permiso>> ListarPorMotivosEntreFechas(IReadOnlyCollection<int> motivos, DateTime desde, DateTime hasta);

    /// <summary>RF-07 a RF-12: permisos de los motivos indicados, de un empleado o de todos, filtrados en la BD.</summary>
    Task<List<Permiso>> ListarPorMotivos(IReadOnlyCollection<int> motivos, int? idEmpleado);
}

/// <summary>RF-14 / RF-16 / SC-005: periodos de teletrabajo filtrados en la BD.</summary>
public interface IPeriodoTeletrabajoRepositorio : IRepositorio<PeriodoTeletrabajo>
{
    /// <summary>Periodos que se cruzan con el rango (límites incluidos), opcionalmente de un empleado y una modalidad.</summary>
    Task<List<PeriodoTeletrabajo>> ListarEntreFechas(int? idEmpleado, bool? parcial, DateOnly desde, DateOnly hasta);
}

public interface IPersonaRepositorio : IRepositorio<Persona>
{
    Task<bool> ExisteDocumento(string numDoc, int? excluirIdPersona = null);
}

public interface IUsuarioRepositorio : IRepositorio<Usuario>
{
    Task<Usuario?> ObtenerPorLogin(string login);
    Task<List<Rol>> ObtenerRoles(int idUsuario);
}

public interface IAsistenciaRepositorio : IRepositorio<Asistencia>
{
    Task<int> ContarEntreFechas(DateTime desde, DateTime hasta);

    /// <summary>SC-003: registros de asistencia de un rango de fechas, filtrados en la BD.</summary>
    Task<List<Asistencia>> ListarEntreFechas(DateTime desde, DateTime hastaExclusivo);
}

/// <summary>Marcaciones del reloj: el filtro y el límite se aplican en la BD (la tabla tiene más de 200 000 filas).</summary>
public interface IMarcacionRepositorio : IRepositorio<Marcacion>
{
    Task<List<Marcacion>> ListarRecientes(int? idEmpleado, DateTime? desde, DateTime? hasta, int maximo);
    Task<int> ContarDiasPorDispositivos(int idEmpleado, DateTime desde, DateTime hasta, string[] dispositivos);
}

/// <summary>Registro diario de asistencia: el filtro y el límite se aplican en la BD (más de 250 000 filas).</summary>
public interface IRegAsisDiarioRepositorio : IRepositorio<RegAsisDiario>
{
    Task<List<RegAsisDiario>> ListarRecientes(int? idEmpleado, DateTime? desde, DateTime? hasta, int maximo);
}

public interface IBitacoraRepositorio
{
    Task Registrar(string usuario, string accion, string entidad, string? detalle = null);
    Task<List<Bitacora>> Ultimas(int cantidad);
    Task<List<DateTime>> Fechas(string usuario, string accion, DateTime desde);
    Task<DateTime?> UltimaFecha(string usuario, string[] acciones);
}

/// <summary>Ejecuta trabajo en una transacción serializable, con reintentos ante bloqueos.</summary>
public interface ISecuenciaService
{
    Task<bool> EjecutarConBloqueoAsync(Func<Task<int>> calcularSiguiente, Func<int, Task<bool>> guardar, CancellationToken ct = default);
    Task<bool> EjecutarTransaccionalAsync(Func<Task> trabajo, CancellationToken ct = default);
}

/// <summary>Columna de un catálogo editable.</summary>
public record CatalogoColumna(string Nombre, string Tipo, bool EsClave, bool EsArchivo, bool EsAutonumerico = false);

/// <summary>Mantenimiento genérico de catálogos (tablas simples) por nombre de tabla.</summary>
public interface ICatalogoRepositorio
{
    bool EsEditable(string tabla);
    Task<List<object>> Listar(string tabla);
    Task<object?> Obtener(string tabla, string[] ids);
    IReadOnlyList<CatalogoColumna> Columnas(string tabla);
    Task Crear(string tabla, IReadOnlyDictionary<string, string> datos);
    Task Actualizar(string tabla, string[] ids, IReadOnlyDictionary<string, string> datos);
    Task Eliminar(string tabla, string[] ids);
    string[] ClavesPrimarias(string tabla);
}
