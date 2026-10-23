using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("EstudiosRealizado")]
    public class EstudiosRealizado
    {
    [Key]
    public int IdTipoEstudios { get; set; }
    [Key]
    public int IdEmpleado { get; set; }
    [Key]
    public int Correlativo { get; set; }
    public string? CentroEstudios { get; set; }
    public string? Especialidad { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaTermino { get; set; }
    public int? IdEstadoEstudio { get; set; }
    }
}
