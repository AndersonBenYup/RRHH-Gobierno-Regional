using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Empleado_Area")]
    public class Empleado_Area
    {
    [Key]
    public int Year { get; set; }
    [Key]
    public int idAreaOrganiz { get; set; }
    [Key]
    public int IdEmpleado { get; set; }
    }
}
