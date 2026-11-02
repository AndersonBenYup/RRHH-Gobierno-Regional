using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("TipoTrabajador")]
    public class TipoTrabajador
    {
    [Key]
    public int IdTipoTrabajador { get; set; }
    [StringLength(30, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Descripcion { get; set; }
    [StringLength(150, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Observaciones { get; set; }
    }
}
