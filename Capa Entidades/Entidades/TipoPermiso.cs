using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("TipoPermiso")]
    public class TipoPermiso
    {
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int idTipoPermiso { get; set; }
    [StringLength(50, ErrorMessage = "Máximo {1} caracteres.")]
    public string? DescripTipoPermiso { get; set; }
    }
}
