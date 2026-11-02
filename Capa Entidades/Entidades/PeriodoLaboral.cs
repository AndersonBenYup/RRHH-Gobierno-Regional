using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("PeriodoLaboral")]
    public class PeriodoLaboral
    {
    [Key]
    [Range(1, int.MaxValue, ErrorMessage = Mensajes.SeleccioneEmpleado)]
    [Required(ErrorMessage = Mensajes.SeleccioneEmpleado)]
    public int IdEmpleado { get; set; }
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int NroPeriodo { get; set; }
    [StringLength(30, ErrorMessage = "Máximo {1} caracteres.")]
    public string? NumResoIngreInstitu { get; set; }
    [FechaNoAnteriorA(nameof(FechaIngreso), ErrorMessage = Mensajes.FechaFinAnterior)]
    public DateTime? FechaSalida { get; set; }
    public DateTime? FechaResoIngreInstitu { get; set; }
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public DateTime? FechaIngreso { get; set; }
    public bool? Judicial { get; set; }
    [StringLength(2000, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Obs { get; set; }
    }
}
