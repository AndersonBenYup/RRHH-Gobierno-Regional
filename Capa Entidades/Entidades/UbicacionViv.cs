using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("UbicacionViv")]
    public class UbicacionViv
    {
    [Key]
    public int IdUbicacion { get; set; }
    public string? DescripUbicacion { get; set; }
    }
}
