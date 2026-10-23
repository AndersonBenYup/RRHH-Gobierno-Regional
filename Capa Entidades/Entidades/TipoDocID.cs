using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("TipoDocID")]
    public class TipoDocID
    {
    [Key]
    [Column("TipoDocID")]
    public int Nombre { get; set; }
    public string? Descripcion { get; set; }
    }
}
