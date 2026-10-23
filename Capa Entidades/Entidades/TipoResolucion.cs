using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("TipoResolucion")]
    public class TipoResolucion
    {
    [Key]
    public int idTipoResolucion { get; set; }
    public string? DescripTipoResolucion { get; set; }
    }
}
