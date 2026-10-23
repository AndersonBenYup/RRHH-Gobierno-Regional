using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("ExpLaboral")]
    public class ExpLaboral
    {
    [Key]
    public int IdEmpleado { get; set; }
    [Key]
    public int IdExpLab { get; set; }
    public string? NomInstitucion { get; set; }
    public string? Cargo { get; set; }
    public DateTime? FechaIngreso { get; set; }
    public DateTime? FechaEgreso { get; set; }
    public string? UnidadOrganica { get; set; }
    public int? IdTipoInstitucion { get; set; }
    }
}
