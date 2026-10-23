using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Rol")]
    public class Rol
    {
    [Key]
    public int IdRol { get; set; }
    public string? Descripcion { get; set; }
    public string? IdSistema { get; set; }
    }
}
