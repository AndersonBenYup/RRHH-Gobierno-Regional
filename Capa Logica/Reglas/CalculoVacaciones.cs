using System.Globalization;
using System.Text;
using Capa_Entidades;

namespace Capa_Logica.Reglas;

/// <summary>
/// Un permiso de vacaciones ya gozado: rango de fechas y si cuenta como media jornada (0,5 por día).
/// Es media jornada si se registró por horas o si su motivo es de media jornada («1/2 JORNADA VACACIONES»):
/// en la BD esos permisos están con <c>Dia = 1</c> y sin horas.
/// </summary>
public sealed record GoceVacaciones(DateOnly Inicio, DateOnly Fin, bool MediaJornada)
{
    public static GoceVacaciones? Desde(Permiso permiso, bool motivoDeMediaJornada = false)
    {
        if (permiso.FechaInicio is not DateTime inicio)
            return null;
        var fin = permiso.FechaFin ?? inicio;
        var porHoras = permiso.Dia != true && permiso.Hora_Sal is not null && permiso.Hora_Ret is not null;
        return new GoceVacaciones(DateOnly.FromDateTime(inicio), DateOnly.FromDateTime(fin), porHoras || motivoDeMediaJornada);
    }
}

/// <summary>Saldo de vacaciones de un empleado a una fecha de corte.</summary>
public sealed record ResumenVacaciones(
    DateOnly FechaIngreso,
    DateOnly FechaCorte,
    int AniosCumplidos,
    decimal Generadas,
    decimal Gozadas,
    decimal Truncas)
{
    /// <summary>RF-08: días generados que faltan gozar. Negativo si se adelantaron vacaciones.</summary>
    public decimal Pendientes => Generadas - Gozadas;
}

/// <summary>
/// RF-07 a RF-10: cálculo de vacaciones. Regla verificada con el área usuaria: 30 días calendario por cada año
/// completo de servicio contado desde la fecha de ingreso (D. Leg. 276, D. Leg. 1057 con Ley 29849 y D. Leg. 713:
/// todos los regímenes del gobierno regional dan 30 días). Funciones puras, sin BD.
/// </summary>
public static class CalculoVacaciones
{
    public const int DiasPorAnio = 30;
    public const decimal DiasPorMes = DiasPorAnio / 12m;   // 2,5
    public const decimal MediaJornada = 0.5m;
    public const int HorasMediaJornada = 4;                 // jornada de 8 horas

    public const string MensajeMediaJornada =
        "Las vacaciones por horas deben cubrir al menos media jornada (4 horas).";

    /// <summary>RF-07: años completos de servicio entre el ingreso y el corte (ambas fechas incluidas).</summary>
    public static int AniosCumplidos(DateOnly ingreso, DateOnly corte)
    {
        var finExclusivo = corte.AddDays(1);
        if (finExclusivo <= ingreso)
            return 0;
        var anios = finExclusivo.Year - ingreso.Year;
        if (ingreso.AddYears(anios) > finExclusivo)
            anios--;
        return anios;
    }

    /// <summary>
    /// RF-09: vacaciones truncas, la parte proporcional del año de servicio en curso:
    /// 2,5 días por mes completo y 1/30 de eso por cada día restante. Redondeado a 2 decimales.
    /// </summary>
    public static decimal Truncas(DateOnly ingreso, DateOnly corte)
    {
        var finExclusivo = corte.AddDays(1);
        if (finExclusivo <= ingreso)
            return 0;
        var aniversario = ingreso.AddYears(AniosCumplidos(ingreso, corte));
        var meses = 0;
        while (aniversario.AddMonths(meses + 1) <= finExclusivo)
            meses++;
        var dias = finExclusivo.DayNumber - aniversario.AddMonths(meses).DayNumber;
        return Math.Round(meses * DiasPorMes + dias * DiasPorMes / 30m, 2);
    }

    /// <summary>
    /// RF-10: días que descuenta un goce dentro de [desde, hasta]. Por día completo cuenta cada día calendario;
    /// media jornada (por horas o por motivo) cuenta 0,5 por cada día.
    /// </summary>
    public static decimal DiasGozados(GoceVacaciones goce, DateOnly desde, DateOnly hasta)
    {
        var inicio = goce.Inicio > desde ? goce.Inicio : desde;
        var fin = goce.Fin < hasta ? goce.Fin : hasta;
        if (fin < inicio)
            return 0;
        var dias = fin.DayNumber - inicio.DayNumber + 1;
        return goce.MediaJornada ? dias * MediaJornada : dias;
    }

    /// <summary>RF-07 a RF-10: saldo a la fecha de corte. Solo cuentan los goces entre el ingreso y el corte.</summary>
    public static ResumenVacaciones Calcular(DateOnly ingreso, DateOnly corte, IEnumerable<GoceVacaciones> goces)
    {
        var anios = AniosCumplidos(ingreso, corte);
        var gozadas = goces.Sum(g => DiasGozados(g, ingreso, corte));
        return new ResumenVacaciones(ingreso, corte, anios, anios * DiasPorAnio, gozadas, Truncas(ingreso, corte));
    }

    /// <summary>RF-10: un permiso de vacaciones por horas debe cubrir al menos media jornada.</summary>
    public static bool CumpleMediaJornada(Permiso permiso)
    {
        if (permiso.Dia == true || permiso.Hora_Sal is not TimeOnly salida || permiso.Hora_Ret is not TimeOnly retorno)
            return true;
        return retorno - salida >= TimeSpan.FromHours(HorasMediaJornada);
    }

    /// <summary>
    /// Un motivo es de vacaciones si su descripción contiene "VACACION" (sin importar tildes ni mayúsculas).
    /// Así no se fijan códigos de motivo que cambian entre bases de datos.
    /// </summary>
    public static bool EsMotivoDeVacaciones(string? descripcion)
        => SinTildes(descripcion).Contains("VACACION", StringComparison.OrdinalIgnoreCase);

    /// <summary>Motivo de vacaciones por media jornada, p. ej. «1/2 JORNADA VACACIONES» o «MEDIA JORNADA».</summary>
    public static bool EsMotivoDeMediaJornada(string? descripcion)
    {
        var texto = SinTildes(descripcion);
        return EsMotivoDeVacaciones(texto)
               && (texto.Contains("1/2 JORNADA", StringComparison.OrdinalIgnoreCase)
                   || texto.Contains("MEDIA JORNADA", StringComparison.OrdinalIgnoreCase));
    }

    private static string SinTildes(string? texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            return "";
        var resultado = new StringBuilder();
        foreach (var c in texto.Normalize(NormalizationForm.FormD))
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                resultado.Append(c);
        return resultado.ToString();
    }
}
