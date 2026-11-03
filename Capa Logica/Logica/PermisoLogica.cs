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

    public PermisoLogica(IPermisoRepositorio permisos) : base(permisos) => _permisos = permisos;

    public Task<Permiso?> Obtener(int idEmpleado, int nPermiso) => Repositorio.Obtener(idEmpleado, nPermiso);

    /// <summary>SC-005: filtro por empleado y paginación resueltos en la BD.</summary>
    public Task<Pagina<Permiso>> ListarPaginado(int? idEmpleado, int pagina, int tamanoPagina = TamanoPagina)
        => _permisos.ListarPaginado(idEmpleado, pagina, tamanoPagina);

    /// <summary>SC-005: siguiente número correlativo del empleado calculado en la BD.</summary>
    public Task<int> SiguienteNumero(int idEmpleado) => _permisos.SiguienteNumero(idEmpleado);

    /// <summary>SC-008: reglas de negocio (fechas, horas y solapamiento) que se revisan antes de guardar.</summary>
    public async Task<List<ErrorValidacion>> Validar(Permiso permiso)
        => ReglasPermiso.Validar(permiso, await _permisos.ListarPorEmpleado(permiso.IdEmpleado));
}
