using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Discapacidad")]
    public class Discapacidad
    {
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int IdDiscapacidad { get; set; }
    [StringLength(12, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Descrip_Discapacidad { get; set; }
    }
}
