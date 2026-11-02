using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("TitulosEmpleado")]
    public class TitulosEmpleado
    {
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int IdTitulo { get; set; }
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    [Range(1, int.MaxValue, ErrorMessage = Mensajes.SeleccioneEmpleado)]
    public int? IdEmpleado { get; set; }
    [StringLength(20, ErrorMessage = "Máximo {1} caracteres.")]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public string? NumColegiatura { get; set; }
    [StringLength(100, ErrorMessage = "Máximo {1} caracteres.")]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public string? DenominacionGrado { get; set; }
    [StringLength(60, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Institucion { get; set; }
    public DateTime? Fecha { get; set; }
    }
}
