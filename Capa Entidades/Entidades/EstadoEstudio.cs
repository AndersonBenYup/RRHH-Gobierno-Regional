using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("EstadoEstudio")]
    public class EstadoEstudio
    {
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int IdEstadoEstudio { get; set; }
    [StringLength(10, ErrorMessage = "Máximo {1} caracteres.")]
    public string? DescripEstadoEstudio { get; set; }
    }
}
