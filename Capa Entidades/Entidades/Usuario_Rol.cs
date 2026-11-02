using Capa_Entidades.Validacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Usuario_Rol")]
    public class Usuario_Rol
    {
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int IdUsuario { get; set; }
    [Key]
    [Required(ErrorMessage = Mensajes.Obligatorio)]
    public int IdRol { get; set; }
    }
}
