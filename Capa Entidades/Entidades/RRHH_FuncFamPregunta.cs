using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("RRHH_FuncFamPregunta")]
    public class RRHH_FuncFamPregunta
    {
    [Key]
    public byte IdPregunta { get; set; }
    public string Codigo { get; set; } = "";
    public string? Dimension { get; set; }
    public byte Orden { get; set; }
    }
}
