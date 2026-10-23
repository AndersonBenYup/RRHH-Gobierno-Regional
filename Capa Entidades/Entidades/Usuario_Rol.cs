using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Usuario_Rol")]
    public class Usuario_Rol
    {
    [Key]
    public int IdUsuario { get; set; }
    [Key]
    public int IdRol { get; set; }
    }
}
