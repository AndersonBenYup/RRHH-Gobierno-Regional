using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("ExcretasViv")]
    public class ExcretasViv
    {
    [Key]
    public int IdExcretas { get; set; }
    public string? DescripExcretas { get; set; }
    }
}
