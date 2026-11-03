using System.ComponentModel.DataAnnotations;

namespace Capa_Logica.Seguridad;

/// <summary>
/// Sección "Seguridad" de la configuración (appsettings, user-secrets o App Settings de Azure con "Seguridad__…").
/// Program.cs la enlaza con IOptions y la valida al arrancar; la Capa Lógica la recibe ya resuelta.
/// </summary>
public sealed class OpcionesSeguridad
{
    public const string Seccion = "Seguridad";

    /// <summary>Clave del usuario admin inicial. Solo se usa si el usuario aún no existe.</summary>
    public string? AdminClave { get; set; }

    /// <summary>Intentos fallidos seguidos que bloquean la cuenta.</summary>
    [Range(1, 20, ErrorMessage = "Seguridad:MaxIntentosFallidos debe estar entre {1} y {2}.")]
    public int MaxIntentosFallidos { get; set; } = ConstantesSeguridad.MaxIntentosFallidos;

    /// <summary>Ventana en que se cuentan los fallos y duración del bloqueo, en minutos.</summary>
    [Range(1, 1440, ErrorMessage = "Seguridad:MinutosBloqueo debe estar entre {1} y {2}.")]
    public int MinutosBloqueo { get; set; } = (int)ConstantesSeguridad.DuracionBloqueo.TotalMinutes;

    public TimeSpan DuracionBloqueo => TimeSpan.FromMinutes(MinutosBloqueo);
}
