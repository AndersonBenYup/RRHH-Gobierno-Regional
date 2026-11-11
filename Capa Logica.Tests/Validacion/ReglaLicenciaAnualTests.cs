using Capa_Entidades;
using Capa_Logica.Reglas;

namespace Capa_Logica.Tests.Validacion;

public class ReglaLicenciaAnualTests
{
    [Theory]
    [InlineData(70, 2026, 7, 0, true)]
    [InlineData(76, 2026, 7, 0, true)]
    [InlineData(82, 2027, 7, 0, false)]
    [InlineData(133, 2026, 8, 0, false)]
    [InlineData(70, 2026, 7, 1, false)]
    [InlineData(125, 2026, 7, 0, false)]
    public void Compara_grupo_empleado_y_anio_sin_contar_el_mismo_registro(
        int motivo, int anio, int empleado, int numero, bool esperado)
    {
        var existente = new Permiso { IdEmpleado = 7, NPermiso = 1, idMotivo = 70, FechaInicio = new(2026, 1, 1) };
        var solicitado = new Permiso { IdEmpleado = empleado, NPermiso = numero, idMotivo = motivo, FechaInicio = new(anio, 6, 1) };

        Assert.Equal(esperado, ReglaLicenciaAnual.TieneOtraLicenciaDelMismoAnio(solicitado, [existente]));
    }
}
