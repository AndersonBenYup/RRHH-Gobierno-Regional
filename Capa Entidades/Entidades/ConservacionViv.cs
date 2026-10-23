using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("ConservacionViv")]
    public class ConservacionViv
    {
    [Key]
    public int IdConservacion { get; set; }
    public string? DescripConserva { get; set; }
    }
}
