using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Capacitacion")]
    public class Capacitacion
    {
    [Key]
    public int IdCapacitacion { get; set; }
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    [Range(1, int.MaxValue, ErrorMessage = Mensajes.SeleccioneEmpleado)]
    public int? IdEmpleado { get; set; }
    [StringLength(300, ErrorMessage = "Máximo {1} caracteres.")]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public string? NombreCurso { get; set; }
    [StringLength(300, ErrorMessage = "Máximo {1} caracteres.")]
    public string? InstitucionOrganizadora { get; set; }
    public DateTime? FechaInicio { get; set; }
    [FechaNoAnteriorA(nameof(FechaInicio), ErrorMessage = Mensajes.FechaFinAnterior)]
    public DateTime? FechaFin { get; set; }
    public short? NumHoras { get; set; }
    }
}
