using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Comportamiento")]
    public class Comportamiento
    {
    [Key]
    public int idComportamiento { get; set; }
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public DateTime? Fecha { get; set; }
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    [Range(1, int.MaxValue, ErrorMessage = Mensajes.SeleccioneEmpleado)]
    public int? IdEmpleado { get; set; }
    [StringLength(200, ErrorMessage = "Máximo {1} caracteres.")]
    public string? NumResol { get; set; }
    [StringLength(1000, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Titulo { get; set; }
    [StringLength(2000, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Descripcion { get; set; }
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int? idTipoComportamiento { get; set; }
    }
}
