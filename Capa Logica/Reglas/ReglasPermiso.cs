using Capa_Entidades;
using Capa_Entidades.Validacion;

namespace Capa_Logica.Reglas;

/// <summary>
/// SC-008 (retoma SC-001): reglas de negocio de los permisos. Son funciones puras para poder probarlas sin BD.
/// </summary>
public static class ReglasPermiso
{
    public const string MensajeFechas = "La fecha de fin no puede ser anterior a la fecha de inicio.";
    public const string MensajeHoras = "La hora de retorno debe ser posterior a la hora de salida.";
    public const string MensajeSinInicio = "Ingrese la fecha de inicio.";

    /// <summary>
    /// Valida un permiso contra los demás permisos del mismo empleado.
    /// <paramref name="otros"/> puede incluir al propio permiso (se descarta por NPermiso).
    /// </summary>
    public static List<ErrorValidacion> Validar(Permiso permiso, IEnumerable<Permiso> otros)
    {
        var errores = new List<ErrorValidacion>();

        if (permiso.FechaInicio is null)
        {
            errores.Add(new(nameof(Permiso.FechaInicio), MensajeSinInicio));
            return errores;
        }

        var inicio = permiso.FechaInicio.Value.Date;
        var fin = (permiso.FechaFin ?? permiso.FechaInicio).Value.Date;
        if (fin < inicio)
            errores.Add(new(nameof(Permiso.FechaFin), MensajeFechas));

        if (inicio == fin && permiso.Hora_Sal is not null && permiso.Hora_Ret is not null
            && permiso.Hora_Ret <= permiso.Hora_Sal)
            errores.Add(new(nameof(Permiso.Hora_Ret), MensajeHoras));

        if (errores.Count > 0)
            return errores;

        var choque = otros.FirstOrDefault(o =>
            o.IdEmpleado == permiso.IdEmpleado
            && o.NPermiso != permiso.NPermiso
            && SeSolapan(permiso, o));
        if (choque is not null)
            errores.Add(new("", $"El permiso se cruza con el permiso N.º {choque.NPermiso} del mismo empleado " +
                                $"({Rango(choque)}). Cambie las fechas u horas."));

        return errores;
    }

    /// <summary>
    /// Dos permisos se solapan si sus días se cruzan y, cuando ambos son por horas en un solo día,
    /// también se cruzan sus horarios.
    /// </summary>
    public static bool SeSolapan(Permiso a, Permiso b)
    {
        if (a.FechaInicio is null || b.FechaInicio is null)
            return false;

        var aIni = a.FechaInicio.Value.Date;
        var aFin = (a.FechaFin ?? a.FechaInicio).Value.Date;
        var bIni = b.FechaInicio.Value.Date;
        var bFin = (b.FechaFin ?? b.FechaInicio).Value.Date;
        if (aIni > bFin || bIni > aFin)
            return false;

        if (EsPorHoras(a) && EsPorHoras(b) && aIni == aFin && bIni == bFin)
            return a.Hora_Sal < b.Hora_Ret && b.Hora_Sal < a.Hora_Ret;

        return true;
    }

    private static bool EsPorHoras(Permiso p)
        => p.Dia != true && p.Hora_Sal is not null && p.Hora_Ret is not null;

    private static string Rango(Permiso p)
    {
        var texto = p.FechaInicio!.Value.ToString("dd/MM/yyyy");
        if (p.FechaFin is not null && p.FechaFin.Value.Date != p.FechaInicio.Value.Date)
            texto += " al " + p.FechaFin.Value.ToString("dd/MM/yyyy");
        if (EsPorHoras(p))
            texto += " de " + p.Hora_Sal!.Value.ToString("HH:mm") + " a " + p.Hora_Ret!.Value.ToString("HH:mm");
        return texto;
    }
}
