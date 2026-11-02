using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("AlumbradoViv")]
    public class AlumbradoViv
    {
    [Key]
    public int IdAlumbrado { get; set; }
    [StringLength(20, ErrorMessage = "Máximo {1} caracteres.")]
    public string? DescripAlumbrado { get; set; }
    }
}
