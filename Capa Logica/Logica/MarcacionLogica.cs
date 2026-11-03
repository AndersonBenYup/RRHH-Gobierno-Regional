using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class MarcacionLogica : LogicaBase<Marcacion>
{
    /// <summary>Máximo de filas que muestra el listado; para ver más, se filtra por empleado o fechas.</summary>
    public const int MaximoListado = 2000;

    private readonly IMarcacionRepositorio _registros;

    public MarcacionLogica(IMarcacionRepositorio registros) : base(registros) => _registros = registros;

    /// <summary>Los más recientes primero, filtrados en la BD (antes se traía la tabla completa a memoria).</summary>
    public Task<List<Marcacion>> ListarRecientes(int? idEmpleado, DateTime? desde, DateTime? hasta)
        => _registros.ListarRecientes(idEmpleado, desde, hasta, MaximoListado);

    public Task<Marcacion?> Obtener(DateTime FechaParam, int IdEmpleadoParam) => Repositorio.Obtener(FechaParam, IdEmpleadoParam);
}
