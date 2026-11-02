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
    [StringLength(40, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Descripcion { get; set; }
    }
}
