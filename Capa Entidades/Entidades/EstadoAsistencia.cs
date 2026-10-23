using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("EstadoAsistencia")]
    public class EstadoAsistencia
    {
    [Key]
    public string Codigo { get; set; } = "";
    public string? Descripcion { get; set; }
    }
}
