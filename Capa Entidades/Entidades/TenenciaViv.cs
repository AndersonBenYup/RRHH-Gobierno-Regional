using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("TenenciaViv")]
    public class TenenciaViv
    {
    [Key]
    public int IdTenencia { get; set; }
    public string? DescripTenencia { get; set; }
    }
}
