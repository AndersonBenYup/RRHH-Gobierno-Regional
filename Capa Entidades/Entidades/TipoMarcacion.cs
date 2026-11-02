using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("TipoMarcacion")]
    public class TipoMarcacion
    {
    [Key]
    [StringLength(1, ErrorMessage = "Máximo {1} caracteres.")]
    public string Codigo { get; set; } = "";
    [StringLength(50, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Descripcion { get; set; }
    }
}
