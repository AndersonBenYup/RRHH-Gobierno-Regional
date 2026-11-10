using Capa_Entidades;

namespace Capa_Logica.Reglas;

public static class ReglaLicenciaAnual
{
    public static bool TieneOtraLicenciaDelMismoAnio(Permiso permiso, IEnumerable<Permiso> registrados)
    {
        if (!EsLicenciaAnual(permiso.idMotivo) || permiso.FechaInicio is not DateTime inicio)
            return false;

        return registrados.Any(otro => otro.IdEmpleado == permiso.IdEmpleado
            && (permiso.NPermiso <= 0 || otro.NPermiso != permiso.NPermiso)
            && EsLicenciaAnual(otro.idMotivo)
            && otro.FechaInicio?.Year == inicio.Year);
    }

    private static bool EsLicenciaAnual(int? motivo) => motivo is 70 or 76 or 82 or 133;
}
