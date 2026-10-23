using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Asistencia")]
    public class Asistencia
    {
    [Key]
    public DateTime Fecha { get; set; }
    [Key]
    public int IdEmpleado { get; set; }
    public TimeOnly? Hor_Ent { get; set; }
    public TimeOnly? Hor_Sal { get; set; }
    public TimeOnly? Alm_Sal { get; set; }
    public TimeOnly? Alm_Ent { get; set; }
    public bool? Flag_Ent { get; set; }
    public bool? Flag_Sal { get; set; }
    public bool? Estado { get; set; }
    public int? idHorario { get; set; }
    public string? Usuario { get; set; }
    public string? PC { get; set; }
    }
}
