using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("RRHH_FuncFamRespuesta")]
    public class RRHH_FuncFamRespuesta
    {
    [Key]
    public int IdEmpleado { get; set; }
    [Key]
    public byte IdPregunta { get; set; }
    [StringLength(2, ErrorMessage = "Máximo {1} caracteres.")]
    public string Respuesta { get; set; } = "";
    }
}
