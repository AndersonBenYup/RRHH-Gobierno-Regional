using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("SistemaOpcion")]
    public class SistemaOpcion
    {
    [Key]
    public string? IdSistemaOpcion { get; set; }
    public string? Descripcion { get; set; }
    }
}
