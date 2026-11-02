using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("ViveCon")]
    public class ViveCon
    {
    [Key]
    public int IdViveCon { get; set; }
    [StringLength(27, ErrorMessage = "Máximo {1} caracteres.")]
    public string? DescripViveCon { get; set; }
    }
}
