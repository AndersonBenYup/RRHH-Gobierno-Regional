using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("TipoInstitucion")]
    public class TipoInstitucion
    {
    [Key]
    public int IdTipoInstitucion { get; set; }
    [StringLength(15, ErrorMessage = "Máximo {1} caracteres.")]
    public string? DescripTipoInstitucion { get; set; }
    }
}
