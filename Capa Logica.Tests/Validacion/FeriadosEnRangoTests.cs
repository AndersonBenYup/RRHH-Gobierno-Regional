using Capa_Entidades;
using Capa_Logica.Reglas;

namespace Capa_Logica.Tests.Validacion;

public class FeriadosEnRangoTests
{
    [Fact]
    public void Cuenta_una_vez_cada_fecha_dentro_de_los_limites()
    {
        RRHH_Feriado[] feriados =
        [
            new() { Fecha = new(2026, 11, 9) },
            new() { Fecha = new(2026, 11, 10) },
            new() { Fecha = new(2026, 11, 10, 12, 0, 0) },
            new() { Fecha = new(2026, 11, 15) },
            new() { Fecha = new(2026, 11, 16) },
            new()
        ];

        Assert.Equal(new DateTime[] { new(2026, 11, 10), new(2026, 11, 15) },
            FeriadosEnRango.Fechas(feriados, new(2026, 11, 10), new(2026, 11, 16)));
    }
}
