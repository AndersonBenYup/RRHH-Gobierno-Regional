namespace Proyecto_Evo_RRLL.Models.ViewModels;

/// <summary>Una opción del dock de navegación inferior. <paramref name="Permiso"/> es el controller que se consulta con PuedeVer.</summary>
public sealed record OpcionMenu(string Etiqueta, string Icono, string Controller, string Action = "Index", string? Permiso = null);

/// <summary>Un grupo del dock (como los menús del sistema de escritorio: Personal, Mantenimientos, Reportes…).</summary>
public sealed record GrupoMenu(string Clave, string Etiqueta, string Icono, string[] Controllers, IReadOnlyList<OpcionMenu> Opciones, bool SoloAdministrador = false);

/// <summary>
/// Definición única del menú: el dock inferior (_DockNavegacion) lo recorre y filtra cada opción con User.PuedeVer.
/// Mantenimiento no se lista aquí porque usa CatalogoNavegacion.Grupos (panel ancho con los catálogos agrupados).
/// </summary>
public static class MenuNavegacion
{
    public static readonly IReadOnlyList<GrupoMenu> Grupos =
    [
        new("personal", "Personal", "bi-people", ["Empleado", "EstructuraOrganiz", "HistorialLaboral", "Capacitacion", "Comportamiento"],
        [
            new("Relación de personal", "bi-person-lines-fill", "Empleado"),
            new("Nuevo empleado", "bi-person-plus", "Empleado", "Nuevo"),
            new("Estructura orgánica", "bi-diagram-3", "EstructuraOrganiz"),
            new("Historial laboral", "bi-briefcase", "HistorialLaboral"),
            new("Capacitación", "bi-mortarboard", "Capacitacion"),
            new("Comportamiento", "bi-clipboard-data", "Comportamiento")
        ]),
        new("asistencia", "Asistencia", "bi-clock-history", ["Asistencia", "Feriado"],
        [
            new("Resumen de asistencia", "bi-clock-history", "Asistencia"),
            new("Marcaciones", "bi-fingerprint", "Asistencia", "Marcaciones"),
            new("Registro diario", "bi-journal-check", "Asistencia", "RegDiario"),
            new("Horarios temporales", "bi-calendar-range", "Asistencia", "HorariosTemporal"),
            new("Feriados", "bi-calendar-event", "Feriado")
        ]),
        new("permisos", "Permisos", "bi-calendar-check", ["Permiso", "Vacaciones", "MisVacaciones"],
        [
            new("Permisos", "bi-calendar-check", "Permiso"),
            new("Teletrabajo", "bi-laptop", "Permiso", "Teletrabajo"),
            new("Vacaciones", "bi-sun", "Vacaciones"),
            new("Mis vacaciones", "bi-person-badge", "MisVacaciones")
        ]),
        new("bienestar", "Bienestar", "bi-heart-pulse", ["Salud", "Familia", "FamiliaSalud", "Vivienda"],
        [
            new("Salud", "bi-heart-pulse", "Salud"),
            new("Asegurados", "bi-shield-plus", "Salud", "Asegurados"),
            new("Familia", "bi-people-fill", "Familia"),
            new("Ficha social", "bi-card-checklist", "FamiliaSalud"),
            new("Vivienda", "bi-house", "Vivienda")
        ]),
        new("reportes", "Reportes", "bi-bar-chart-line", ["Reportes"],
        [
            new("Centro de reportes", "bi-grid-3x3-gap", "Reportes"),
            new("Ausencias", "bi-person-dash", "Permiso", "ReporteAusencias"),
            new("Teletrabajo", "bi-laptop", "Permiso", "ReporteTeletrabajo"),
            new("Saldo de vacaciones", "bi-sun", "Vacaciones")
        ]),
        new("seguridad", "Seguridad", "bi-shield-lock", ["SeguridadAdmin"],
        [
            new("Usuarios", "bi-person-gear", "SeguridadAdmin", "Usuarios"),
            new("Roles", "bi-person-vcard", "SeguridadAdmin", "Roles"),
            new("Bitácora de actividad", "bi-clock-history", "SeguridadAdmin", "Actividad")
        ], SoloAdministrador: true)
    ];

    /// <summary>
    /// Pestañas de las sub-pantallas de cada módulo (las dibuja _PestanasModulo sobre el contenido).
    /// Solo aparecen en las pantallas de lista que figuran aquí, no en los formularios.
    /// Reemplazan las filas de botones "Marcaciones / Registro diario / …" que cada vista repetía.
    /// </summary>
    public static readonly IReadOnlyDictionary<string, IReadOnlyList<OpcionMenu>> Pestanas =
        new Dictionary<string, IReadOnlyList<OpcionMenu>>(StringComparer.OrdinalIgnoreCase)
        {
            ["Asistencia"] =
            [
                new("Resumen", "bi-clipboard-check", "Asistencia"),
                new("Marcaciones", "bi-fingerprint", "Asistencia", "Marcaciones"),
                new("Registro diario", "bi-journal-check", "Asistencia", "RegDiario"),
                new("Horarios temporales", "bi-calendar-range", "Asistencia", "HorariosTemporal")
            ],
            ["Permiso"] =
            [
                new("Permisos", "bi-calendar-check", "Permiso"),
                new("Teletrabajo", "bi-laptop", "Permiso", "Teletrabajo"),
                new("Reporte de ausencias", "bi-person-dash", "Permiso", "ReporteAusencias"),
                new("Reporte de teletrabajo", "bi-bar-chart", "Permiso", "ReporteTeletrabajo")
            ],
            ["Salud"] =
            [
                new("Ficha de salud", "bi-heart-pulse", "Salud"),
                new("Asegurados", "bi-shield-plus", "Salud", "Asegurados"),
                new("A quién acudir", "bi-telephone", "Salud", "AcudeEnferm")
            ],
            ["Familia"] =
            [
                new("Familiares", "bi-people-fill", "Familia"),
                new("Dinámica familiar", "bi-diagram-3", "Familia", "Dinamica"),
                new("Funcionalidad familiar", "bi-sliders", "Familia", "FuncFam")
            ],
            ["Capacitacion"] =
            [
                new("Capacitaciones", "bi-mortarboard", "Capacitacion"),
                new("Estudios", "bi-journal-text", "Capacitacion", "Estudios"),
                new("Títulos", "bi-award", "Capacitacion", "Titulos")
            ],
            ["EstructuraOrganiz"] =
            [
                new("Unidades orgánicas", "bi-diagram-3", "EstructuraOrganiz"),
                new("Locales", "bi-building", "EstructuraOrganiz", "Locales")
            ],
            ["SeguridadAdmin"] =
            [
                new("Usuarios", "bi-person-gear", "SeguridadAdmin", "Usuarios"),
                new("Roles", "bi-person-vcard", "SeguridadAdmin", "Roles"),
                new("Bitácora de actividad", "bi-clock-history", "SeguridadAdmin", "Actividad")
            ]
        };
}
