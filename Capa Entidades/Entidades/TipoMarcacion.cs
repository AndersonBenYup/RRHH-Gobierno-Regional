using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("TipoMarcacion")]
    public class TipoMarcacion
    {
    [Key]
    public string Codigo { get; set; } = "";
    public string? Descripcion { get; set; }
    }
}
