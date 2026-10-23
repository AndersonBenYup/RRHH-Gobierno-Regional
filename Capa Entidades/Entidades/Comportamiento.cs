using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Comportamiento")]
    public class Comportamiento
    {
    [Key]
    public int idComportamiento { get; set; }
    public DateTime? Fecha { get; set; }
    public int? IdEmpleado { get; set; }
    public string? NumResol { get; set; }
    public string? Titulo { get; set; }
    public string? Descripcion { get; set; }
    public int? idTipoComportamiento { get; set; }
    }
}
