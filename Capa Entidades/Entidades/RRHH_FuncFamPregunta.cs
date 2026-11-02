using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("RRHH_FuncFamPregunta")]
    public class RRHH_FuncFamPregunta
    {
    [Key]
    public byte IdPregunta { get; set; }
    [StringLength(6, ErrorMessage = "Máximo {1} caracteres.")]
    public string Codigo { get; set; } = "";
    [StringLength(30, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Dimension { get; set; }
    public byte Orden { get; set; }
    }
}
