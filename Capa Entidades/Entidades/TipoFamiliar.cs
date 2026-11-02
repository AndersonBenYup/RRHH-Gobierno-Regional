using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("TipoFamiliar")]
    public class TipoFamiliar
    {
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int IdTipoFam { get; set; }
    [StringLength(10, ErrorMessage = "Máximo {1} caracteres.")]
    public string? DescripTipoFam { get; set; }
    }
}
