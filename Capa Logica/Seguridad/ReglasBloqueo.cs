namespace Capa_Logica.Seguridad;

/// <summary>Estado de bloqueo de una cuenta en un momento dado.</summary>
public sealed record EstadoBloqueo(bool Bloqueada, int FallosRecientes, DateTime? BloqueadaHasta)
{
    public static readonly EstadoBloqueo Libre = new(false, 0, null);

    public int MinutosRestantes(DateTime ahora)
        => BloqueadaHasta is null ? 0 : Math.Max(1, (int)Math.Ceiling((BloqueadaHasta.Value - ahora).TotalMinutes));
}

/// <summary>
/// SC-009 (completa SC-002): una cuenta se bloquea al acumular <see cref="ConstantesSeguridad.MaxIntentosFallidos"/>
/// fallos dentro de la ventana de <see cref="ConstantesSeguridad.DuracionBloqueo"/>, contados desde el último
/// acceso correcto o desbloqueo. El bloqueo dura hasta <c>último fallo + duración</c>.
/// Es una función pura para poder probarla sin BD.
/// </summary>
public static class ReglasBloqueo
{
    /// <param name="fallos">Fechas de los intentos fallidos registrados.</param>
    /// <param name="ultimoReinicio">Último acceso correcto o desbloqueo por el administrador (null si no hay).</param>
    public static EstadoBloqueo Evaluar(IEnumerable<DateTime> fallos, DateTime? ultimoReinicio, DateTime ahora,
        int maxIntentos = ConstantesSeguridad.MaxIntentosFallidos, TimeSpan? duracion = null)
    {
        var ventana = duracion ?? ConstantesSeguridad.DuracionBloqueo;
        var desde = ahora - ventana;
        if (ultimoReinicio is not null && ultimoReinicio > desde)
            desde = ultimoReinicio.Value;

        var recientes = fallos.Where(f => f > desde && f <= ahora).OrderBy(f => f).ToList();
        if (recientes.Count < maxIntentos)
            return new EstadoBloqueo(false, recientes.Count, null);

        var hasta = recientes[^1] + ventana;
        return hasta > ahora
            ? new EstadoBloqueo(true, recientes.Count, hasta)
            : new EstadoBloqueo(false, recientes.Count, null);
    }
}
