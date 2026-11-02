using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Rotacion")]
    public class Rotacion
    {
    [Key]
    [Range(1, int.MaxValue, ErrorMessage = Mensajes.SeleccioneEmpleado)]
    [Required(ErrorMessage = Mensajes.SeleccioneEmpleado)]
    public int idEmpleado { get; set; }
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int idRotacion { get; set; }
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public DateOnly? FechaMemo { get; set; }
    [StringLength(50, ErrorMessage = "Máximo {1} caracteres.")]
    public string? NroMemo { get; set; }
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int? idAreaOrganiz { get; set; }
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int Year { get; set; }
    }
}
