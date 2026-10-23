using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Capacitacion")]
    public class Capacitacion
    {
    [Key]
    public int IdCapacitacion { get; set; }
    public int? IdEmpleado { get; set; }
    public string? NombreCurso { get; set; }
    public string? InstitucionOrganizadora { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public short? NumHoras { get; set; }
    }
}
