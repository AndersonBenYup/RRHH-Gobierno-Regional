using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("vw_FamiliarDetalle")]
    public class FamiliarDetalle
    {
    [Key]
    public int IdEmpleado { get; set; }
    [Key]
    public int IdPersona { get; set; }
    public int? IdTipoFam { get; set; }
    public bool? ViveCasa { get; set; }
    public bool? LaboraInstitucion { get; set; }
    }
}
