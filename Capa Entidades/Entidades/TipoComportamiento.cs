using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("TipoComportamiento")]
    public class TipoComportamiento
    {
    [Key]
    public int idTipoComportamiento { get; set; }
    public string? TipoCompDescrip { get; set; }
    }
}
