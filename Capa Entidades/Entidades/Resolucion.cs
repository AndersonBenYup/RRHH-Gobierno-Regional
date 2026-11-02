using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Resolucion")]
    public class Resolucion
    {
    [Key]
    [Range(1, int.MaxValue, ErrorMessage = Mensajes.SeleccioneEmpleado)]
    [Required(ErrorMessage = Mensajes.SeleccioneEmpleado)]
    public int idEmpleado { get; set; }
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int idResolucion { get; set; }
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int? idTipoResolucion { get; set; }
    [StringLength(50, ErrorMessage = "Máximo {1} caracteres.")]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public string? NumeroResolucion { get; set; }
    public DateOnly? FechaResolucion { get; set; }
    public DateOnly? FechaVigencia { get; set; }
    }
}
