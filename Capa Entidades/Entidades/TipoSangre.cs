using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("TipoSangre")]
    public class TipoSangre
    {
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int IdTipoSangre { get; set; }
    [StringLength(5, ErrorMessage = "Máximo {1} caracteres.")]
    public string? DescripTS { get; set; }
    }
}
