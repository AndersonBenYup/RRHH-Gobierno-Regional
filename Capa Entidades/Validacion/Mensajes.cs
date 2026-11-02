namespace Capa_Entidades.Validacion;

/// <summary>SC-008: textos de validación compartidos entre atributos y reglas de negocio.</summary>
public static class Mensajes
{
    public const string Obligatorio = "Este campo es obligatorio.";
    public const string Largo = "Máximo {1} caracteres.";
    public const string SeleccioneEmpleado = "Seleccione el empleado.";
    public const string FechaFinAnterior = "La fecha de fin no puede ser anterior a la fecha de inicio.";
}

/// <summary>Error de una regla de negocio: el campo afectado ("" = general) y su mensaje.</summary>
public sealed record ErrorValidacion(string Campo, string Mensaje);
