using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("TipoPermiso")]
    public class TipoPermiso
    {
    [Key]
    public int idTipoPermiso { get; set; }
    public string? DescripTipoPermiso { get; set; }
    }
}
