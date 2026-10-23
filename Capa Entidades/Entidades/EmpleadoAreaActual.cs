using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("vw_EmpleadoAreaActual")]
    public class EmpleadoAreaActual
    {
    [Key]
    public int IdEmpleado { get; set; }
    public int Year { get; set; }
    public int idAreaOrganiz { get; set; }
    public string? AreaOrganizacional { get; set; }
    }
}
