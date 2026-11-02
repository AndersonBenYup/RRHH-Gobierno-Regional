using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("RRHH_FuncFamPregunta")]
    public class RRHH_FuncFamPregunta
    {
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public byte IdPregunta { get; set; }
    [StringLength(6, ErrorMessage = "Máximo {1} caracteres.")]
    public string Codigo { get; set; } = "";
    [StringLength(30, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Dimension { get; set; }
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public byte Orden { get; set; }
    }
}
