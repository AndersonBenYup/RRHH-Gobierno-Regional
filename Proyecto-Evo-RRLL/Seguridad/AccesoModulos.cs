using System.Security.Claims;
using Capa_Logica.Seguridad;

namespace Proyecto_Evo_RRLL.Seguridad;

/// <summary>
/// SC-009: permisos por opción. Al iniciar sesión se guarda en la cookie un claim por módulo permitido
/// (o "*" si el usuario ve todo); el filtro global y el menú lateral solo leen esos claims.
/// </summary>
public static class AccesoModulos
{
    public const string TipoClaim = "evo:modulo";
    public const string Todos = "*";

    public static IEnumerable<Claim> Claims(AccesoUsuario acceso)
    {
        if (acceso.EsAdministrador)
            yield return new Claim(ClaimTypes.Role, ReglasAcceso.RolAdministrador);
        if (acceso.VeTodo)
        {
            yield return new Claim(TipoClaim, Todos);
            yield break;
        }
        foreach (var codigo in acceso.Modulos!.Order())
            yield return new Claim(TipoClaim, codigo);
    }

    /// <summary>true si el usuario puede abrir el controller (los que no son de un módulo están siempre permitidos).</summary>
    public static bool PuedeVer(this ClaimsPrincipal usuario, string controller)
    {
        var modulo = ReglasAcceso.ModuloDe(controller);
        if (modulo is null)
            return true;
        return usuario.IsInRole(ReglasAcceso.RolAdministrador)
               || usuario.HasClaim(TipoClaim, Todos)
               || usuario.HasClaim(TipoClaim, modulo.Codigo);
    }
}
