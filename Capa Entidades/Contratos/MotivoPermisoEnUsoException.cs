namespace Capa_Entidades.Contratos;

public sealed class MotivoPermisoEnUsoException(Exception? causa = null)
    : InvalidOperationException("No se puede eliminar el motivo porque ya está asociado a un permiso.", causa)
{
}
