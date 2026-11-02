using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("TipoComportamiento")]
    public class TipoComportamiento
    {
    [Key]
    public int idTipoComportamiento { get; set; }
    [StringLength(8, ErrorMessage = "Máximo {1} caracteres.")]
    public string? TipoCompDescrip { get; set; }
    }
}
