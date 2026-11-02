using Capa_Entidades.Validacion;
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
    [Range(1, int.MaxValue, ErrorMessage = Mensajes.SeleccioneEmpleado)]
    public int IdEmpleado { get; set; }
    public short? MinNormales { get; set; }
    public int? SegundosTarde { get; set; }
    [StringLength(3, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Estado { get; set; }
    }
}
