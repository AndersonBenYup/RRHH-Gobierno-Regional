using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Rotacion")]
    public class Rotacion
    {
    [Key]
    public int idEmpleado { get; set; }
    [Key]
    public int idRotacion { get; set; }
    public DateOnly? FechaMemo { get; set; }
    public string? NroMemo { get; set; }
    public int? idAreaOrganiz { get; set; }
    public int Year { get; set; }
    }
}
