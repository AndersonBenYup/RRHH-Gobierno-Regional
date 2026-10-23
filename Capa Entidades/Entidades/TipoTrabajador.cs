using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("TipoTrabajador")]
    public class TipoTrabajador
    {
    [Key]
    public int IdTipoTrabajador { get; set; }
    public string? Descripcion { get; set; }
    public string? Observaciones { get; set; }
    }
}
