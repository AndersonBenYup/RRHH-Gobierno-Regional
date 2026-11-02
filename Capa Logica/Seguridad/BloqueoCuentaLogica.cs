using Capa_Entidades.Contratos;

namespace Capa_Logica.Seguridad;

/// <summary>
/// SC-009: bloqueo de cuenta por intentos fallidos. Los intentos se guardan como eventos en la bitácora
/// (no hace falta cambiar el esquema de la tabla Usuario) y así quedan auditados.
/// </summary>
public class BloqueoCuentaLogica
{
    private static readonly string[] AccionesReinicio =
        [ConstantesSeguridad.AccionLoginCorrecto, ConstantesSeguridad.AccionDesbloquear];

    private readonly IBitacoraRepositorio _bitacora;
    private readonly TimeProvider _reloj;

    public BloqueoCuentaLogica(IBitacoraRepositorio bitacora, TimeProvider reloj)
    {
        _bitacora = bitacora;
        _reloj = reloj;
    }

    public DateTime Ahora => _reloj.GetLocalNow().DateTime;

    public async Task<EstadoBloqueo> Estado(string login)
    {
        var ahora = Ahora;
        var desde = ahora - ConstantesSeguridad.DuracionBloqueo;
        var fallos = await _bitacora.Fechas(login, ConstantesSeguridad.AccionLoginFallido, desde);
        if (fallos.Count < ConstantesSeguridad.MaxIntentosFallidos)
            return new EstadoBloqueo(false, fallos.Count, null);

        var reinicio = await _bitacora.UltimaFecha(login, AccionesReinicio);
        return ReglasBloqueo.Evaluar(fallos, reinicio, ahora);
    }

    /// <summary>Registra un intento fallido y devuelve el estado resultante (registra también si la cuenta quedó bloqueada).</summary>
    public async Task<EstadoBloqueo> RegistrarFallo(string login, string? ip)
    {
        await _bitacora.Registrar(login, ConstantesSeguridad.AccionLoginFallido, ConstantesSeguridad.EntidadSeguridad,
            $"Clave incorrecta o usuario inexistente. IP={ip ?? "?"}");

        var estado = await Estado(login);
        if (estado.Bloqueada)
            await _bitacora.Registrar(login, ConstantesSeguridad.AccionCuentaBloqueada, ConstantesSeguridad.EntidadSeguridad,
                $"{estado.FallosRecientes} intentos fallidos. Bloqueada hasta {estado.BloqueadaHasta:dd/MM/yyyy HH:mm}.");
        return estado;
    }

    public Task RegistrarIntentoBloqueado(string login, string? ip)
        => _bitacora.Registrar(login, ConstantesSeguridad.AccionLoginBloqueado, ConstantesSeguridad.EntidadSeguridad,
            $"Intento con la cuenta bloqueada. IP={ip ?? "?"}");

    /// <summary>El administrador desbloquea la cuenta: los fallos anteriores dejan de contar.</summary>
    public Task Desbloquear(string login, string administrador)
        => _bitacora.Registrar(login, ConstantesSeguridad.AccionDesbloquear, ConstantesSeguridad.EntidadSeguridad,
            $"Desbloqueada por {administrador}.");
}
