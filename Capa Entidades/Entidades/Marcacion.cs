using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Marcacion")]
    public class Marcacion
    {
    [Key]
    public DateTime Fecha { get; set; }
    [Key]
    public int IdEmpleado { get; set; }
    public byte NroMarcacion { get; set; } = 1;
    public string TipoMarcacion { get; set; } = "N";
    public string? CodDispositivo { get; set; }
    public int? idHorario { get; set; }
    }
}
