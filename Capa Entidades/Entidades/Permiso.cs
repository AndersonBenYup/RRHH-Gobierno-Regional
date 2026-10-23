using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Permiso")]
    public class Permiso
    {
    [Key]
    public int IdEmpleado { get; set; }
    [Key]
    public int NPermiso { get; set; }
    public int? idMotivo { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public TimeOnly? Hora_Sal { get; set; }
    public TimeOnly? Hora_Ret { get; set; }
    public bool? Dia { get; set; }
    public bool? Retorno { get; set; }
    public string? Lugar { get; set; }
    public string? Referencia { get; set; }
    public string? Autorizacion { get; set; }
    public string? AutorizacionRRHH { get; set; }
    public bool? Autorizado { get; set; }
    public string? Obs { get; set; }
    }
}
