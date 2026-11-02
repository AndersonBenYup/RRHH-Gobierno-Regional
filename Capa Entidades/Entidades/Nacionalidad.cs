using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Nacionalidad")]
    public class Nacionalidad
    {
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int IdNacionalidad { get; set; }
    [StringLength(50, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Descripcion { get; set; }
    [StringLength(3, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Abreviatura { get; set; }
    }
}
