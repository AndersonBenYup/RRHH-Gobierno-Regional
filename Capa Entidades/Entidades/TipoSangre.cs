using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("TipoSangre")]
    public class TipoSangre
    {
    [Key]
    public int IdTipoSangre { get; set; }
    public string? DescripTS { get; set; }
    }
}
