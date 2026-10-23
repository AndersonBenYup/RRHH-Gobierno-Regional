using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Resolucion")]
    public class Resolucion
    {
    [Key]
    public int idEmpleado { get; set; }
    [Key]
    public int idResolucion { get; set; }
    public int? idTipoResolucion { get; set; }
    public string? NumeroResolucion { get; set; }
    public DateOnly? FechaResolucion { get; set; }
    public DateOnly? FechaVigencia { get; set; }
    }
}
