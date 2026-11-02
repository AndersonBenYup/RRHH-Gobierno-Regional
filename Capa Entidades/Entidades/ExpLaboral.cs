using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("ExpLaboral")]
    public class ExpLaboral
    {
    [Key]
    [Range(1, int.MaxValue, ErrorMessage = Mensajes.SeleccioneEmpleado)]
    [Required(ErrorMessage = Mensajes.SeleccioneEmpleado)]
    public int IdEmpleado { get; set; }
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int IdExpLab { get; set; }
    [StringLength(120, ErrorMessage = "Máximo {1} caracteres.")]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public string? NomInstitucion { get; set; }
    [StringLength(40, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Cargo { get; set; }
    public DateTime? FechaIngreso { get; set; }
    [FechaNoAnteriorA(nameof(FechaIngreso), ErrorMessage = Mensajes.FechaFinAnterior)]
    public DateTime? FechaEgreso { get; set; }
    [StringLength(100, ErrorMessage = "Máximo {1} caracteres.")]
    public string? UnidadOrganica { get; set; }
    public int? IdTipoInstitucion { get; set; }
    }
}
