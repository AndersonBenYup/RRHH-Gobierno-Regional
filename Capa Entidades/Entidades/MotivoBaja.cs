using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("MotivoBaja")]
    public class MotivoBaja
    {
    [Key]
    public int idMotivoBaja { get; set; }
    public string? Descripcion { get; set; }
    }
}
