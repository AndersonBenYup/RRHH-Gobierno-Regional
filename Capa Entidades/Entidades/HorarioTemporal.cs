using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("HorarioTemporal")]
    public class HorarioTemporal
    {
    [Key]
    public int idEmpleado { get; set; }
    [Key]
    public int N { get; set; }
    public DateOnly? FechaInicio { get; set; }
    public DateOnly? FechaFin { get; set; }
    public int? HorarioAsignado { get; set; }
    public string? NroDocumento { get; set; }
    public string? Sisgedo { get; set; }
    public DateTime? FechaRegistro { get; set; }
    }
}
