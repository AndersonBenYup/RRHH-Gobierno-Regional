using Capa_Entidades;

namespace Capa_Logica.Reglas;

public static class DiasTeletrabajo
{
    public static int Contar(IEnumerable<PeriodoTeletrabajo> periodos, DateOnly desde, DateOnly hasta)
    {
        var rangos = periodos
            .Where(p => p.FechaInicio.HasValue && p.FechaFin.HasValue)
            .Select(p => (Inicio: Math.Max(p.FechaInicio!.Value.DayNumber, desde.DayNumber),
                Fin: Math.Min(p.FechaFin!.Value.DayNumber, hasta.DayNumber)))
            .Where(r => r.Inicio <= r.Fin)
            .OrderBy(r => r.Inicio);

        var ultimoDia = desde.DayNumber - 1;
        var total = 0;
        foreach (var rango in rangos)
        {
            if (rango.Fin <= ultimoDia)
                continue;
            total += rango.Fin - Math.Max(rango.Inicio, ultimoDia + 1) + 1;
            ultimoDia = rango.Fin;
        }

        return total;
    }
}
