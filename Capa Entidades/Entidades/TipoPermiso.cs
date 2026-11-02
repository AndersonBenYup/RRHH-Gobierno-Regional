using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("TipoPermiso")]
    public class TipoPermiso
    {
    [Key]
    public int idTipoPermiso { get; set; }
    [StringLength(50, ErrorMessage = "Máximo {1} caracteres.")]
    public string? DescripTipoPermiso { get; set; }
    }
}
