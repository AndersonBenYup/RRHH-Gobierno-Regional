using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Marcacion")]
    public class Marcacion
    {
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public DateTime Fecha { get; set; }
    [Key]
    [Range(1, int.MaxValue, ErrorMessage = Mensajes.SeleccioneEmpleado)]
    [Required(ErrorMessage = Mensajes.SeleccioneEmpleado)]
    public int IdEmpleado { get; set; }
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public byte NroMarcacion { get; set; } = 1;
    [StringLength(1, ErrorMessage = "Máximo {1} caracteres.")]
    public string TipoMarcacion { get; set; } = "N";
    [StringLength(10, ErrorMessage = "Máximo {1} caracteres.")]
    public string? CodDispositivo { get; set; }
    public int? idHorario { get; set; }
    }
}
