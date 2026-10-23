using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("DispositivoMarcacion")]
    public class DispositivoMarcacion
    {
    [Key]
    public string Codigo { get; set; } = "";
    public string? Descripcion { get; set; }
    }
}
