using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("MaterialViv")]
    public class MaterialViv
    {
    [Key]
    public int IdMaterial { get; set; }
    public string? DescripMaterial { get; set; }
    }
}
