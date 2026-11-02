using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Permiso")]
    public class Permiso
    {
    [Key]
    [Range(1, int.MaxValue, ErrorMessage = Mensajes.SeleccioneEmpleado)]
    [Required(ErrorMessage = Mensajes.SeleccioneEmpleado)]
    public int IdEmpleado { get; set; }
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int NPermiso { get; set; }
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int? idMotivo { get; set; }
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public DateTime? FechaInicio { get; set; }
    [FechaNoAnteriorA(nameof(FechaInicio), ErrorMessage = Mensajes.FechaFinAnterior)]
    public DateTime? FechaFin { get; set; }
    public TimeOnly? Hora_Sal { get; set; }
    public TimeOnly? Hora_Ret { get; set; }
    public bool? Dia { get; set; }
    public bool? Retorno { get; set; }
    [StringLength(20, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Lugar { get; set; }
    [StringLength(100, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Referencia { get; set; }
    [StringLength(50, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Autorizacion { get; set; }
    [StringLength(50, ErrorMessage = "Máximo {1} caracteres.")]
    public string? AutorizacionRRHH { get; set; }
    public bool? Autorizado { get; set; }
    [StringLength(200, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Obs { get; set; }
    }
}
