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
    public string? EnferCronicaDescrip { get; set; }
    public bool? FamDiscapacidad { get; set; }
    public string? FamDiscapDescrip { get; set; }
    public bool? Alergico { get; set; }
    public string? AlergicoDescrip { get; set; }
    public int? idAcudeEnferm { get; set; }
    }
}
