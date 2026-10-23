using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("EstadoEstudio")]
    public class EstadoEstudio
    {
    [Key]
    public int IdEstadoEstudio { get; set; }
    public string? DescripEstadoEstudio { get; set; }
    }
}
