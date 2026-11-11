namespace Capa_Logica.Seguridad;

/// <summary>Un módulo de RRHH que se puede dar o quitar a un rol (tabla SistemaOpcion, bajo "0500000 RRHH").</summary>
public sealed record ModuloRrhh(string Codigo, string Nombre, string[] Controllers);

/// <summary>Lo que un usuario puede abrir: todo (administrador o rol sin configurar) o solo ciertos módulos.</summary>
public sealed record AccesoUsuario(bool EsAdministrador, IReadOnlySet<string>? Modulos)
{
    /// <summary>true si el usuario ve todos los módulos (administrador o algún rol sin módulos configurados).</summary>
    public bool VeTodo => EsAdministrador || Modulos is null;

    public bool Puede(string codigoModulo) => VeTodo || Modulos!.Contains(codigoModulo);
}

/// <summary>
/// SC-009: permisos por opción (Rol_Acceso). Reglas puras, sin base de datos:
/// <list type="bullet">
/// <item>Administrador de RRHH: rol "Administrador" (el que crea el sistema) o "ADMINISTRADOR" del sistema RRHH (0500000).
/// Los administradores de otros sistemas del gobierno regional (SIA, SIAB, Consejo Regional…) ya no lo son aquí.</item>
/// <item>Un rol sin módulos de RRHH configurados ve todo (así nadie pierde acceso hasta que el administrador lo configure).</item>
/// <item>Un rol con módulos configurados ve solo esos; con varios roles se suman.</item>
/// </list>
/// </summary>
public static class ReglasAcceso
{
    public const string SistemaRrhh = "0500000";
    public const string SistemaInicial = "GRLL"; // el que usa el admin que crea Program.cs si no hay ninguno
    public const string RolAdministrador = "Administrador";

    public static readonly IReadOnlyList<ModuloRrhh> Modulos =
    [
        new("0502001", "Personal", ["Empleado"]),
        new("0502002", "Salud", ["Salud"]),
        new("0502003", "Familia", ["Familia", "FamiliaSalud"]),
        new("0502004", "Asistencia", ["Asistencia"]),
        new("0502005", "Feriados", ["Feriado"]),
        new("0502006", "Capacitación", ["Capacitacion"]),
        new("0502007", "Permisos", ["Permiso", "Vacaciones"]), // RF-07 a RF-11: el reporte de vacaciones es parte de Permisos
        new("0502008", "Historial laboral", ["HistorialLaboral"]),
        new("0502009", "Comportamiento", ["Comportamiento"]),
        new("0502010", "Vivienda", ["Vivienda"]),
        new("0502011", "Estructura orgánica", ["EstructuraOrganiz"]),
        new("0502012", "Mantenimiento", ["Catalogo"])
    ];

    private static readonly HashSet<string> CodigosModulo = Modulos.Select(m => m.Codigo).ToHashSet();

    public static bool EsCodigoModulo(string? codigo) => codigo is not null && CodigosModulo.Contains(codigo.Trim());

    /// <summary>Módulo al que pertenece un controller; null si no está restringido (Inicio, sesión, ayuda…).</summary>
    public static ModuloRrhh? ModuloDe(string? controller)
        => controller is null ? null : Modulos.FirstOrDefault(m => m.Controllers.Contains(controller, StringComparer.OrdinalIgnoreCase));

    public static bool EsAdministradorRrhh(string? descripcionRol, string? idSistema)
    {
        var descripcion = descripcionRol?.Trim();
        var sistema = idSistema?.Trim();
        if (string.Equals(descripcion, RolAdministrador, StringComparison.Ordinal))
            return true;
        return string.Equals(descripcion, RolAdministrador, StringComparison.OrdinalIgnoreCase)
               && (sistema == SistemaRrhh || sistema == SistemaInicial);
    }

    /// <param name="roles">Roles del usuario (Id, descripción, sistema).</param>
    /// <param name="accesos">Filas de Rol_Acceso (IdRol, IdSistemaOpcion) de esos roles; las de otros sistemas se ignoran.</param>
    public static AccesoUsuario Calcular(IEnumerable<(int IdRol, string? Descripcion, string? IdSistema)> roles,
        IEnumerable<(int IdRol, string? IdSistemaOpcion)> accesos)
    {
        var listaRoles = roles.ToList();
        if (listaRoles.Any(r => EsAdministradorRrhh(r.Descripcion, r.IdSistema)))
            return new AccesoUsuario(true, null);
        if (listaRoles.Count == 0)
            return new AccesoUsuario(false, null);

        var modulosPorRol = accesos
            .Where(a => EsCodigoModulo(a.IdSistemaOpcion))
            .GroupBy(a => a.IdRol)
            .ToDictionary(g => g.Key, g => g.Select(a => a.IdSistemaOpcion!.Trim()).ToHashSet());

        // Un rol sin módulos configurados ve todo.
        if (listaRoles.Any(r => !modulosPorRol.ContainsKey(r.IdRol)))
            return new AccesoUsuario(false, null);

        return new AccesoUsuario(false, listaRoles.SelectMany(r => modulosPorRol[r.IdRol]).ToHashSet());
    }
}
