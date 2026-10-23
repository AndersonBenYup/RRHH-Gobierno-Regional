using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("EstadoCivil")]
    public class EstadoCivil
    {
    [Key]
    public int IdEstadoCivil { get; set; }
    public string? DescripEstCivil { get; set; }
    public string? AbrevEstCiv { get; set; }
    }
}
