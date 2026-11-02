using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("HorarioTemporal")]
    public class HorarioTemporal
    {
    [Key]
    [Range(1, int.MaxValue, ErrorMessage = Mensajes.SeleccioneEmpleado)]
    public int idEmpleado { get; set; }
    [Key]
    public int N { get; set; }
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public DateOnly? FechaInicio { get; set; }
    [FechaNoAnteriorA(nameof(FechaInicio), ErrorMessage = Mensajes.FechaFinAnterior)]
    public DateOnly? FechaFin { get; set; }
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int? HorarioAsignado { get; set; }
    [StringLength(50, ErrorMessage = "Máximo {1} caracteres.")]
    public string? NroDocumento { get; set; }
    [StringLength(50, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Sisgedo { get; set; }
    public DateTime? FechaRegistro { get; set; }
    }
}
