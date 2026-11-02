using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("ConservacionViv")]
    public class ConservacionViv
    {
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int IdConservacion { get; set; }
    [StringLength(17, ErrorMessage = "Máximo {1} caracteres.")]
    public string? DescripConserva { get; set; }
    }
}
