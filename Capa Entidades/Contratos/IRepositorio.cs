namespace Capa_Entidades.Contratos;

/// <summary>
/// SC-006: contrato genérico de acceso a datos. La Capa Lógica solo conoce esta interfaz;
/// la implementación con EF Core vive en Capa Datos (RepositorioBase&lt;T&gt;), así que
/// cambiar de base de datos solo toca Capa Datos.
/// </summary>
public interface IRepositorio<T> where T : class
{
    Task<List<T>> Listar();

    /// <summary>Busca por clave primaria; con clave compuesta, los valores van en el orden de la clave.</summary>
    Task<T?> Obtener(params object[] claves);

    Task Crear(T registro);
    Task Actualizar(T registro);
    Task Eliminar(T registro);
}
