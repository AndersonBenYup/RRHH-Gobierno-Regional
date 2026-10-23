using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("TipoEstudio")]
    public class TipoEstudio
    {
    [Key]
    public int IdTipoEstudios { get; set; }
    public string? DescripTipoEst { get; set; }
    }
}
