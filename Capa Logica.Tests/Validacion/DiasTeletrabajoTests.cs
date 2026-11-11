using Capa_Entidades;
using Capa_Logica.Reglas;

namespace Capa_Logica.Tests.Validacion;

public class DiasTeletrabajoTests
{
    [Fact]
    public void Recorta_el_rango_y_cuenta_los_dias_superpuestos_solo_una_vez()
    {
        PeriodoTeletrabajo[] periodos =
        [
            new() { FechaInicio = new(2026, 11, 8), FechaFin = new(2026, 11, 12) },
            new() { FechaInicio = new(2026, 11, 11), FechaFin = new(2026, 11, 18), Parcial = true },
            new() { FechaInicio = new(2026, 11, 20), FechaFin = new(2026, 11, 21) }
        ];

        Assert.Equal(6, DiasTeletrabajo.Contar(periodos, new(2026, 11, 10), new(2026, 11, 15)));
    }

    [Fact]
    public void Sin_periodos_devuelve_cero()
    {
        Assert.Equal(0, DiasTeletrabajo.Contar([], new(2026, 11, 10), new(2026, 11, 10)));
    }
}
