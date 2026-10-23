using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("TipoInstitucion")]
    public class TipoInstitucion
    {
    [Key]
    public int IdTipoInstitucion { get; set; }
    public string? DescripTipoInstitucion { get; set; }
    }
}
