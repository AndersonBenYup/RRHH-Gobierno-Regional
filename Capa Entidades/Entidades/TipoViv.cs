using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("TipoViv")]
    public class TipoViv
    {
    [Key]
    public int IdTipo { get; set; }
    [StringLength(20, ErrorMessage = "Máximo {1} caracteres.")]
    public string? DescripTipo { get; set; }
    }
}
