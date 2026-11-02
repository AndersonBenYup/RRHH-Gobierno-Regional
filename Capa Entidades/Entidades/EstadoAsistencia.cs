using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("EstadoAsistencia")]
    public class EstadoAsistencia
    {
    [Key]
    [StringLength(3, ErrorMessage = "Máximo {1} caracteres.")]
    public string Codigo { get; set; } = "";
    [StringLength(80, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Descripcion { get; set; }
    }
}
