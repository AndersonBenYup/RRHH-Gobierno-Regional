using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Empleado_Area")]
    public class Empleado_Area
    {
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int Year { get; set; }
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int idAreaOrganiz { get; set; }
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int IdEmpleado { get; set; }
    }
}
