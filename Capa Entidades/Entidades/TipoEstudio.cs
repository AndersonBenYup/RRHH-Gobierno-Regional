using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("TipoEstudio")]
    public class TipoEstudio
    {
    [Key]
    public int IdTipoEstudios { get; set; }
    [StringLength(15, ErrorMessage = "Máximo {1} caracteres.")]
    public string? DescripTipoEst { get; set; }
    }
}
