using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("vw_RRHH_FuncFam")]
    public class RRHH_FuncFam
    {
    [Key]
    public int IdEmpleado { get; set; }
    public string? Com1 { get; set; }
    public string? Com2 { get; set; }
    public string? Afec1 { get; set; }
    public string? Afec2 { get; set; }
    public string? Decis1 { get; set; }
    public string? Decis2 { get; set; }
    public string? Soc1 { get; set; }
    public string? Soc2 { get; set; }
    public string? Soc3 { get; set; }
    }
}
