using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("ViviendaServicio")]
    public class ViviendaServicio
    {
    [Key]
    public int IdVivienda { get; set; }
    [Key]
    public int IdServicio { get; set; }
    }
}
