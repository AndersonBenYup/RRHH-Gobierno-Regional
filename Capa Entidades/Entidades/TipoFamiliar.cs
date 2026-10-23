using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("TipoFamiliar")]
    public class TipoFamiliar
    {
    [Key]
    public int IdTipoFam { get; set; }
    public string? DescripTipoFam { get; set; }
    }
}
