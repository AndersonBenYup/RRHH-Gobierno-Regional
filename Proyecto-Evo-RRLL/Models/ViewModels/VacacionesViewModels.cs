using System.Globalization;
using Capa_Logica;

namespace Proyecto_Evo_RRLL.Models.ViewModels;

/// <summary>RF-11: una fila del reporte de vacaciones.</summary>
public sealed record VacacionesFila(string Empleado, VacacionesEmpleado Datos);

/// <summary>RF-11: totales del reporte (todas las páginas).</summary>
public sealed record VacacionesTotales(decimal Generadas, decimal Gozadas, decimal Pendientes, decimal Truncas);

/// <summary>RF-12: lo que ve el empleado en "Mis vacaciones".</summary>
public sealed record MisVacacionesModelo(string Nombre, VacacionesEmpleado? Vacaciones, string? Aviso);

public static class VacacionesFormato
{
    private static readonly CultureInfo Peru = CultureInfo.GetCultureInfo("es-PE");

    /// <summary>Días con hasta dos decimales (las medias jornadas y las truncas no son enteras).</summary>
    public static string Dias(decimal valor) => valor.ToString("0.##", Peru);
}
