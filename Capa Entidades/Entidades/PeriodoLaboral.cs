using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("PeriodoLaboral")]
    public class PeriodoLaboral
    {
    [Key]
    public int IdEmpleado { get; set; }
    [Key]
    public int NroPeriodo { get; set; }
    public string? NumResoIngreInstitu { get; set; }
    public DateTime? FechaSalida { get; set; }
    public DateTime? FechaResoIngreInstitu { get; set; }
    public DateTime? FechaIngreso { get; set; }
    public bool? Judicial { get; set; }
    public string? Obs { get; set; }
    }
}
