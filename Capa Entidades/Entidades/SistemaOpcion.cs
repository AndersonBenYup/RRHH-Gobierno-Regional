using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("SistemaOpcion")]
    public class SistemaOpcion
    {
    [Key]
    [StringLength(7, ErrorMessage = "Máximo {1} caracteres.")]
    public string? IdSistemaOpcion { get; set; }
    [StringLength(150, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Descripcion { get; set; }
    }
}
