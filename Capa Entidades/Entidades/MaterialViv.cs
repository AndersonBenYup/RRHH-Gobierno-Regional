using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("MaterialViv")]
    public class MaterialViv
    {
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int IdMaterial { get; set; }
    [StringLength(12, ErrorMessage = "Máximo {1} caracteres.")]
    public string? DescripMaterial { get; set; }
    }
}
