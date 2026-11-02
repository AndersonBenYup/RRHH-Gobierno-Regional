using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Familiar")]
    public class Familiar
    {
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int IdEmpleado { get; set; }
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int IdPersona { get; set; }
    public int? IdTipoFam { get; set; }
    public bool? ViveCasa { get; set; }
    }
}
