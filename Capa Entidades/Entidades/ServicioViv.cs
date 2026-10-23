using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("ServicioViv")]
    public class ServicioViv
    {
    [Key]
    public int IdServicio { get; set; }
    public string? Descripcion { get; set; }
    }
}
