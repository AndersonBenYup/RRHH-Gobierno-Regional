using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Local")]
    public class Local
    {
    [Key]
    public int idLocal { get; set; }
    [StringLength(50, ErrorMessage = "Máximo {1} caracteres.")]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public string? NombreLocal { get; set; }
    public bool? Estado { get; set; }
    }
}
