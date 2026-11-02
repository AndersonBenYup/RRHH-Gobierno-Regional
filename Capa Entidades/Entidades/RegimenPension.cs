using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("RegimenPension")]
    public class RegimenPension
    {
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int IdRegimenPen { get; set; }
    [StringLength(5, ErrorMessage = "Máximo {1} caracteres.")]
    public string? LeyRegimen { get; set; }
    }
}
