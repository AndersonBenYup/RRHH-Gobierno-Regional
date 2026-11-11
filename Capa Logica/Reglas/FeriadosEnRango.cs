using Capa_Entidades;

namespace Capa_Logica.Reglas;

public static class FeriadosEnRango
{
    public static List<DateTime> Fechas(IEnumerable<RRHH_Feriado> feriados, DateTime desde, DateTime hastaExclusivo)
        => feriados.Where(f => f.Fecha.HasValue)
            .Select(f => f.Fecha!.Value.Date)
            .Where(fecha => fecha >= desde && fecha < hastaExclusivo)
            .Distinct()
            .OrderBy(fecha => fecha)
            .ToList();
}
