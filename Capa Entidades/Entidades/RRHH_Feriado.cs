using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("RRHH_Feriado")]
    public class RRHH_Feriado
    {
    [Key]
    public int idFeriado { get; set; }
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public DateTime? Fecha { get; set; }
    [StringLength(200, ErrorMessage = "Máximo {1} caracteres.")]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public string? Motivo { get; set; }
    }
}
