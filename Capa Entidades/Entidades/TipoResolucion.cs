using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("TipoResolucion")]
    public class TipoResolucion
    {
    [Key]
    public int idTipoResolucion { get; set; }
    [StringLength(50, ErrorMessage = "Máximo {1} caracteres.")]
    public string? DescripTipoResolucion { get; set; }
    }
}
