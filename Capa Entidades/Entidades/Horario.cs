using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Horario")]
    public class Horario
    {
    [Key]
    public int idHorario { get; set; }
    public string? Descrip_Horario { get; set; }
    public TimeOnly? Ingreso1 { get; set; }
    public TimeOnly? Salida1 { get; set; }
    public TimeOnly? Ingreso2 { get; set; }
    public TimeOnly? Salida2 { get; set; }
    public bool? SgtDia { get; set; }
    public bool? Estado { get; set; }
    }
}
