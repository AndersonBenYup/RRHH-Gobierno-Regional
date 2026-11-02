namespace Capa_Logica.Seguridad;

/// <summary>SC-009: parámetros del bloqueo de cuenta y nombres de los eventos que se guardan en la bitácora.</summary>
public static class ConstantesSeguridad
{
    /// <summary>Intentos fallidos seguidos que bloquean la cuenta.</summary>
    public const int MaxIntentosFallidos = 5;

    /// <summary>Ventana en la que se cuentan los fallos y duración del bloqueo.</summary>
    public static readonly TimeSpan DuracionBloqueo = TimeSpan.FromMinutes(15);

    public const string EntidadSeguridad = "Seguridad";
    public const string AccionLoginCorrecto = "IniciarSesion";
    public const string AccionLoginFallido = "LoginFallido";
    public const string AccionLoginBloqueado = "LoginBloqueado";
    public const string AccionCuentaBloqueada = "CuentaBloqueada";
    public const string AccionDesbloquear = "DesbloquearCuenta";
}
