using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("ServicioViv")]
    public class ServicioViv
    {
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int IdServicio { get; set; }
    [StringLength(30, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Descripcion { get; set; }
    }
}
