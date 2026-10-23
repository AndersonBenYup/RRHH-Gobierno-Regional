using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("RegimenPension")]
    public class RegimenPension
    {
    [Key]
    public int IdRegimenPen { get; set; }
    public string? LeyRegimen { get; set; }
    }
}
