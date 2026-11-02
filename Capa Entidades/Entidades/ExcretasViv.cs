using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("ExcretasViv")]
    public class ExcretasViv
    {
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int IdExcretas { get; set; }
    [StringLength(33, ErrorMessage = "Máximo {1} caracteres.")]
    public string? DescripExcretas { get; set; }
    }
}
