using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("VinculoAsegurado")]
    public class VinculoAsegurado
    {
    [Key]
    public int IdVinculoAsegurado { get; set; }
    public string? Descripcion { get; set; }
    }
}
