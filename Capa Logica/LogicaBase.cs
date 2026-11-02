using Capa_Entidades.Contratos;

namespace Capa_Logica;

/// <summary>
/// SC-006: base de la Capa Lógica. Depende solo del contrato IRepositorio&lt;T&gt; (Capa Entidades),
/// nunca de EF Core ni de Capa Datos.
/// </summary>
public abstract class LogicaBase<T> where T : class
{
    protected readonly IRepositorio<T> Repositorio;

    protected LogicaBase(IRepositorio<T> repositorio) => Repositorio = repositorio;

    public virtual Task<List<T>> Listar() => Repositorio.Listar();
    public virtual Task Crear(T registro) => Repositorio.Crear(registro);
    public virtual Task Actualizar(T registro) => Repositorio.Actualizar(registro);
    public virtual Task Eliminar(T registro) => Repositorio.Eliminar(registro);
}
