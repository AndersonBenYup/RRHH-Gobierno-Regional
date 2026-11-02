using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("RRHH_Salud")]
    public class RRHH_Salud
    {
    [Key]
    public int idSalud { get; set; }
    public int? IdEmpleado { get; set; }
    public bool? EnferCronica { get; set; }
    [StringLength(200, ErrorMessage = "Máximo {1} caracteres.")]
    public string? EnferCronicaDescrip { get; set; }
    public bool? FamDiscapacidad { get; set; }
    [StringLength(200, ErrorMessage = "Máximo {1} caracteres.")]
    public string? FamDiscapDescrip { get; set; }
    public bool? Alergico { get; set; }
    [StringLength(200, ErrorMessage = "Máximo {1} caracteres.")]
    public string? AlergicoDescrip { get; set; }
    public int? idAcudeEnferm { get; set; }
    }
}
