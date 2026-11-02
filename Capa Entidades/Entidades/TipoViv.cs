using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("TipoViv")]
    public class TipoViv
    {
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int IdTipo { get; set; }
    [StringLength(20, ErrorMessage = "Máximo {1} caracteres.")]
    public string? DescripTipo { get; set; }
    }
}
