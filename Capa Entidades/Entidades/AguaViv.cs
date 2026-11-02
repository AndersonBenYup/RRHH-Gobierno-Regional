using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("AguaViv")]
    public class AguaViv
    {
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int IdAgua { get; set; }
    [StringLength(33, ErrorMessage = "Máximo {1} caracteres.")]
    public string? DescripAgua { get; set; }
    }
}
