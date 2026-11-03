using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica.Seguridad;

/// <summary>SC-009: calcula qué módulos de RRHH puede abrir un usuario a partir de sus roles y de Rol_Acceso.</summary>
public class AccesoLogica
{
    private readonly IRepositorio<Rol_Acceso> _accesos;

    public AccesoLogica(IRepositorio<Rol_Acceso> accesos) => _accesos = accesos;

    public async Task<AccesoUsuario> CalcularAsync(IReadOnlyCollection<Rol> roles)
    {
        var idsRol = roles.Select(r => r.IdRol).ToHashSet();
        var accesos = (await _accesos.Listar())
            .Where(a => idsRol.Contains(a.IdRol))
            .Select(a => (a.IdRol, a.IdSistemaOpcion));
        return ReglasAcceso.Calcular(roles.Select(r => (r.IdRol, r.Descripcion, r.IdSistema)), accesos);
    }
}
