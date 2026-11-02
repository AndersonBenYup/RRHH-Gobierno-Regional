using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("EstadoCivil")]
    public class EstadoCivil
    {
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int IdEstadoCivil { get; set; }
    [StringLength(11, ErrorMessage = "Máximo {1} caracteres.")]
    public string? DescripEstCivil { get; set; }
    [StringLength(3, ErrorMessage = "Máximo {1} caracteres.")]
    public string? AbrevEstCiv { get; set; }
    }
}
