using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Rol_Acceso")]
    public class Rol_Acceso
    {
    [Key]
    public int IdRol { get; set; }
    [Key]
    public string? IdSistemaOpcion { get; set; }
    }
}
