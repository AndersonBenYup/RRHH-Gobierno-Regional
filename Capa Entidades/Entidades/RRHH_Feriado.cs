using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("RRHH_Feriado")]
    public class RRHH_Feriado
    {
    [Key]
    public int idFeriado { get; set; }
    public DateTime? Fecha { get; set; }
    public string? Motivo { get; set; }
    }
}
