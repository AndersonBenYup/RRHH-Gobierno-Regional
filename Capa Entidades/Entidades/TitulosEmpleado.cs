using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("TitulosEmpleado")]
    public class TitulosEmpleado
    {
    [Key]
    public int IdTitulo { get; set; }
    public int? IdEmpleado { get; set; }
    public string? NumColegiatura { get; set; }
    public string? DenominacionGrado { get; set; }
    public string? Institucion { get; set; }
    public DateTime? Fecha { get; set; }
    }
}
