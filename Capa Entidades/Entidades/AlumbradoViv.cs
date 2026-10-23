using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("AlumbradoViv")]
    public class AlumbradoViv
    {
    [Key]
    public int IdAlumbrado { get; set; }
    public string? DescripAlumbrado { get; set; }
    }
}
