using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("RegAsisDiario")]
    public class RegAsisDiario
    {
    [Key]
    public DateTime Fecha { get; set; }
    [Key]
    public int IdEmpleado { get; set; }
    public short? MinNormales { get; set; }
    public int? SegundosTarde { get; set; }
    public string? Estado { get; set; }
    }
}
