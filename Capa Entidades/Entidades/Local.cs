using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Local")]
    public class Local
    {
    [Key]
    public int idLocal { get; set; }
    public string? NombreLocal { get; set; }
    public bool? Estado { get; set; }
    }
}
