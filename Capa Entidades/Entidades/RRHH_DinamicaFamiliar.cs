using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("RRHH_DinamicaFamiliar")]
    public class RRHH_DinamicaFamiliar
    {
    [Key]
    public int idDinamica { get; set; }
    public int? IdEmpleado { get; set; }
    public int? idTipoFam { get; set; }
    public int? idRelaPareja { get; set; }
    public int? idRelaPH { get; set; }
    public int? idRelacHermano { get; set; }
    public string? Observacion { get; set; }
    }
}
