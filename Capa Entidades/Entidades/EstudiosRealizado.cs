using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("EstudiosRealizado")]
    public class EstudiosRealizado
    {
    [Key]
    [Range(1, int.MaxValue, ErrorMessage = "Seleccione el tipo de estudio.")]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int IdTipoEstudios { get; set; }
    [Key]
    [Range(1, int.MaxValue, ErrorMessage = Mensajes.SeleccioneEmpleado)]
    [Required(ErrorMessage = Mensajes.SeleccioneEmpleado)]
    public int IdEmpleado { get; set; }
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int Correlativo { get; set; }
    [StringLength(150, ErrorMessage = "Máximo {1} caracteres.")]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public string? CentroEstudios { get; set; }
    [StringLength(100, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Especialidad { get; set; }
    public DateTime? FechaInicio { get; set; }
    [FechaNoAnteriorA(nameof(FechaInicio), ErrorMessage = Mensajes.FechaFinAnterior)]
    public DateTime? FechaTermino { get; set; }
    public int? IdEstadoEstudio { get; set; }
    }
}
